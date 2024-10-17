using System.Reactive.Linq;
using McProtoNet;
using McProtoNet.Client;
using McProtoNet.MultiVersionProtocol;

public class BowBot
{
    private MinecraftClient _client;
    private MultiProtocol protocol;

    public BowBot(string name)
    {
        _client = new MinecraftClient();
        _client.Host = "192.168.0.5";
        _client.Port = 25565;
        _client.Username = name;
        _client.Version = MinecraftVersion.Latest;
        protocol = new MultiProtocol(_client);
    }

    private Guid targetUUID;
    private int targetEntityId = 0;
    private Vector3 targetPos;
    private Vector3 targetVelocity;
    private Vector3 myPos;
    private Rotation myRot;

    public async Task Run()
    {
        var onSpawnPlayer = protocol.OnSpawnEntity;
        var onPlayerDataUpdate = protocol.OnPlayerInfoUpdate;

        onPlayerDataUpdate.Subscribe(p =>
        {
            foreach (var entry in p.Entries)
            {
                if (entry.Value.Profile is not null)
                {
                    if (entry.Value.Profile.Username == "Title_")
                    {
                        targetUUID = entry.Key;
                        return;
                    }
                }
            }
        });
        onSpawnPlayer.Subscribe(s =>
        {
            if (s.ObjectUUID == targetUUID)
            {
                targetEntityId = s.EntityId;
                targetPos = new Vector3(s.X, s.Y, s.Z);
                targetVelocity = new Vector3(s.VelocityX, s.VelocityY, s.VelocityZ);
            }
        });
        protocol.OnLogin.Subscribe(p =>
        {
            protocol.SendClientInformation("ru", 16, 0, true, 0b1111111, 1, true, true);
        });
        protocol.OnPosition.FirstAsync().Subscribe(x =>
        {
            protocol.SendPositionLook(x.X, x.Y, x.Z, x.Yaw, x.Pitch, true);
        });
        protocol.OnPosition.Subscribe(x =>
        {
            myPos = new Vector3(x.X, x.Y, x.Z);
            protocol.SendTeleportConfirm(x.TeleportId);
        });

        protocol.OnEntityMove.Subscribe(x =>
        {
            if (targetEntityId == x.EntityId)
            {
                Vector3 delta = new Vector3(x.DeltaX, x.DeltaY, x.DeltaZ);

                delta /= 4096;
                targetVelocity = delta;
                Vector3 old = targetPos;
                targetPos += delta;
                TargetPosChanged(old);
            }
        });
        protocol.OnEntityTeleport.Subscribe(x =>
        {
            if (targetEntityId == x.EntityId)
            {
                Vector3 old = targetPos;
                targetPos = new Vector3(x.X, x.Y, x.Z);
                TargetPosChanged(old);
            }
        });
        protocol.OnEntityMoveLook.Subscribe(x =>
        {
            if (targetEntityId == x.EntityId)
            {
                Vector3 delta = new Vector3(x.DeltaX, x.DeltaY, x.DeltaZ);
                delta /= 4096;
                targetVelocity = delta;
                Vector3 old = targetPos;
                targetPos += delta;
                TargetPosChanged(old);
            }
        });

        await _client.Start();
        _ = RunInternal();
    }

    private Queue<Vector3> targetPredict = new Queue<Vector3>();

    private void TargetPosChanged(Vector3 oldPos)
    {
        const float Gravity = 20f;
        const float ArrowSpeed = 60f;


        Vector3 speedTarget = (targetPos - oldPos) * 10;
        
        targetPredict.Enqueue(speedTarget);
        if (targetPredict.Count >= 5)
        {
            targetPredict.Dequeue();
            Vector3 average = Vector3.Zero;
            foreach (var v in targetPredict)
            {
                average += v;
            }

            average = average.Normalize() * speedTarget.Distance;
            speedTarget = average;
        }

        BallisticSolver.solve_ballistic_arc(myPos, ArrowSpeed, targetPos, speedTarget, Gravity, out Vector3 a1,
            out Vector3 a2);


        var rot = new Rotation(a1);


        if (float.IsNaN(rot.Pitch))
        {
            return;
        }

        myRot = rot;
        protocol.SendLook(myRot.Yaw, myRot.Pitch, true);
    }

    private async Task RunInternal()
    {
        await Task.Delay(1000);
        int sec = 0;
        while (true)
        {
            await protocol.SendUseItem(0, ++sec, myRot.Yaw, myRot.Pitch);
            await Task.Delay(1000);
            await protocol.SendPlayerAction(5, default, 0, 0);
            await Task.Delay(100);
        }
    }
}