namespace McProtoNet.Protocol;

public class MinecraftSmeltingFormat
{
    public string Group { get; }
    public Ingredient Ingredient { get; }
    public Slot Result { get; }
    public float Experience { get; }
    public int CookTime { get; }
}