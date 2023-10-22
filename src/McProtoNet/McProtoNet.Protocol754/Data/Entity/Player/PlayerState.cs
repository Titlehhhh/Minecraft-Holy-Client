
namespace McProtoNet.Protocol754.Data
{
    public enum PlayerState
    {
        START_SNEAKING,
        STOP_SNEAKING,
        LEAVE_BED,
        START_SPRINTING,
        STOP_SPRINTING,
        START_HORSE_JUMP,
        STOP_HORSE_JUMP,
        OPEN_HORSE_INVENTORY,
        START_ELYTRA_FLYING
    }
    public enum PlayerAction
    {
        START_DIGGING,
        CANCEL_DIGGING,
        FINISH_DIGGING,
        DROP_ITEM_STACK,
        DROP_ITEM,
        RELEASE_USE_ITEM,
        SWAP_HANDS
    }
}
