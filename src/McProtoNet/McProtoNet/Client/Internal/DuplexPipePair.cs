using System.IO.Pipelines;

namespace McProtoNet.Client;

internal sealed class DuplexPipePair
{
    private readonly Pipe pipe1;
    private readonly Pipe pipe2;

    public DuplexPipePair()
    {
        pipe1 = new Pipe();

        pipe2 = new Pipe();

        Transport = new DuplexPipe(pipe1.Reader, pipe2.Writer);


        Application = new DuplexPipe(pipe2.Reader, pipe1.Writer);
    }

    public IDuplexPipe Transport { get; }
    public IDuplexPipe Application { get; }

    public void Reset()
    {
        pipe1.Reset();
        pipe2.Reset();
    }
}