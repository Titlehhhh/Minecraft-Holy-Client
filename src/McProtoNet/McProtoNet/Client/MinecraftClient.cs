using System.Net.Sockets;
using DotNext;
using DotNext.Threading;
using LibDeflate;
using McProtoNet.Abstractions;
using McProtoNet.Protocol;
using QuickProxyNet;

namespace McProtoNet.Client;


public struct MinecraftVersion
{
    public readonly static int Latest = 767;
    
    public int ProtocolVersion { get; }
    public string MajorVersion { get; }
    public string MinorVersion { get; }
    
    
}

public sealed class MinecraftClient : Disposable, IPacketBroker
{
    private MinecraftPacketReader _packetReader;
    private MinecraftPacketSender _packetSender;

    public MinecraftClient()
    {
        // pipePair = new DuplexPipePair();
        //
        //
        // transportHandler = new TransportHandler(pipePair.Transport);
        //
        //
        //
        // packetPipeHandler = new PacketPipeHandler(
        //     pipePair.Application,
        //     compressor,
        //     decompressor);
        //
        // packetPipeHandler.PacketReceived = PacketPipeHahdeler_PacketReceived;
        //
        // mainTask = Task.CompletedTask;

        minecraftLogin.StateChanged += MinecraftLogin_StateChanged;
    }

    public TimeSpan ConnectTimeout { get; set; } = TimeSpan.FromSeconds(3);

    public int ReadTimeout { get; set; } = 30_000;
    public int WriteTimeout { get; set; } = 30_000;

    public async ValueTask SendPacket(ReadOnlyMemory<byte> data)
    {
        var holder = await sendLock.AcquireAsync(CTS.Token);
        try
        {
            await _packetSender.SendPacketAsync(data, CTS.Token);
        }
        catch (Exception ex)
        {
            OnException(ex);
            throw;
        }
        finally
        {
            holder.Dispose();
        }
    }

    private void MinecraftLogin_StateChanged(MinecraftClientState state)
    {
        StateChange(state);
    }

    private void PacketPipeHahdeler_PacketReceived(object sender, InputPacket packet)
    {
        PacketReceived?.Invoke(this, packet);
    }

    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Host))
            throw new ArgumentException("Host is empty");
        if (string.IsNullOrEmpty(Username))
            throw new ArgumentException("Username is empty");

        if (!(Version >= MinVersionSupport && Version <= MaxVersionSupport))
            throw new ArgumentException("Version not supported");
    }

    public async Task Start()
    {
        try
        {
            Validate();


            if (CTS is not null) CTS.Dispose();

            CTS = new CancellationTokenSource();


            StateChange(MinecraftClientState.Connect);
            try
            {
                using (var timeCts = CancellationTokenSource.CreateLinkedTokenSource(CTS.Token))
                {
                    timeCts.CancelAfter(ConnectTimeout);
                    await ConnectAsync(timeCts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                throw new ConnectTimeoutException();
            }

            var loginOptions = new LoginOptions(Host, Port, Version, Username);


            var result = await minecraftLogin.Login(mainStream, loginOptions, CTS.Token);

            _ = MainLoop(result, CTS.Token);
        }
        catch (OperationCanceledException ex)
        {
            if (CTS is not null)
                if (!CTS.IsCancellationRequested)
                    OnException(ex);
        }
        catch (Exception ex)
        {
            OnException(ex);
            throw;
        }
    }

    private void StateChange(MinecraftClientState state)
    {
        var old = _state;
        _state = state;
        StateChanged?.Invoke(this, new StateEventArgs(old, state));
    }


    private void OnException(Exception ex)
    {
        CleanUp();
        StateChanged?.Invoke(this, new StateEventArgs(ex, _state));
    }

    private async Task MainLoop(LoginizationResult loginizationResult, CancellationToken cancellationToken)
    {
        _packetSender = new MinecraftPacketSender();
        _packetReader = new MinecraftPacketReader();

        _packetReader.BaseStream = loginizationResult.Stream;
        _packetSender.BaseStream = loginizationResult.Stream;

        _packetSender.SwitchCompression(loginizationResult.CompressionThreshold);
        _packetReader.SwitchCompression(loginizationResult.CompressionThreshold);

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var packet = await _packetReader.ReadNextPacketAsync(cancellationToken);
                try
                {
                    PacketReceived?.Invoke(this, packet);
                }
                finally
                {
                    packet.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            OnException(ex);
        }
        finally
        {
            _packetSender.Dispose();
            _packetReader.Dispose();
        }
    }


    private async ValueTask ConnectAsync(CancellationToken cancellationToken)
    {
        var newTcp = new TcpClient();

        var register = cancellationToken.Register(s => ((TcpClient)s).Dispose(), newTcp);
        try
        {
            Interlocked.Exchange(ref tcpClient, newTcp)?.Dispose();

            newTcp.ReceiveTimeout = ReadTimeout;
            newTcp.SendTimeout = WriteTimeout;

            newTcp.Client.NoDelay = true;
            newTcp.LingerState = new LingerOption(false, 0);


            if (Proxy is not null)
            {
                await newTcp.ConnectAsync(Proxy.ProxyHost, Proxy.ProxyPort, cancellationToken);
                mainStream = await Proxy.ConnectAsync(newTcp.GetStream(), Host, Port, cancellationToken);
            }
            else
            {
                try
                {
                    await newTcp.Client.DisconnectAsync(true, cancellationToken);
                }
                catch
                {
                }

                await newTcp.ConnectAsync(Host, Port, cancellationToken);
                mainStream = newTcp.GetStream();
            }
        }
        finally
        {
            register.Dispose();
        }
    }


    private void CleanUp()
    {
        minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
        if (CTS is not null)
        {
            CTS.Cancel();
            CTS.Dispose();
            CTS = null;
        }

        Interlocked.Exchange(ref tcpClient, null)?.Dispose();
        Interlocked.Exchange(ref mainStream, null)?.Dispose();
    }

    public void Stop()
    {
        if (_state == MinecraftClientState.Stopped)
            return;

        CleanUp();

        StateChanged?.Invoke(this, new StateEventArgs(MinecraftClientState.Stopped, _state));
    }

    protected override void Dispose(bool disposing)
    {
        GC.SuppressFinalize(this);
        minecraftLogin.StateChanged -= MinecraftLogin_StateChanged;
        Disposed?.Invoke();
        CTS.Dispose();
        mainStream.Dispose();
        tcpClient.Dispose();

        base.Dispose(disposing);
    }

    #region Events

    public event PacketHandler? PacketReceived;
    public event EventHandler<StateEventArgs> StateChanged;
    public event Action? Disposed;

    #endregion

    #region Properties

    public string Host { get; set; }
    public ushort Port { get; set; } = 25565;

    public string Username { get; set; }
    public int Version { get; set; }
    public IProxyClient? Proxy { get; set; }

    #endregion


    #region Constans

    public const int MinVersionSupport = 340;
    public static readonly int MaxVersionSupport = MinecraftVersion.Latest;

    #endregion

    #region Fields

    private Stream mainStream;
    private CancellationTokenSource CTS;


    // private readonly DuplexPipePair pipePair;
    // private readonly TransportHandler transportHandler;
    // private readonly PacketPipeHandler packetPipeHandler;
    //private Task mainTask;



    private readonly MinecraftClientLogin minecraftLogin = new();

    private TcpClient tcpClient;


    private MinecraftClientState _state;

    private readonly AsyncLock sendLock = new();

    #endregion
}

public class ConnectTimeoutException : Exception
{
}