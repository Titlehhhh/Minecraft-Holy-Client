using McProtoNet.Protocol340.Data.World.Chunk;

namespace McProtoNet.Protocol340.Data
{
    public record class BlockChangeRecord(Vector3 Position, BlockState State);

}
