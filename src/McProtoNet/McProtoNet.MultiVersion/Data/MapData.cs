namespace McProtoNet.MultiVersion
{
    public class MapData
    {
        public byte Columns { get; private set; }
        public byte Rows { get; private set; }
        public byte X { get; private set; }
        public byte Y { get; private set; }
        public byte[] Data { get; private set; }

        public MapData(byte columns, byte rows, byte x, byte y, byte[] data)
        {
            Columns = columns;
            Rows = rows;
            X = x;
            Y = y;
            Data = data;
        }
    }
}
