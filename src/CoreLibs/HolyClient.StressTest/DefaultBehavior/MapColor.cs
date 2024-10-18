namespace HolyClient.StressTest;

public sealed class MapColor
{
    public static readonly MapColor[] Colors = new MapColor[64];
    public static readonly MapColor NONE = new MapColor(0, 0);
    public static readonly MapColor GRASS = new MapColor(1, 8368696);
    public static readonly MapColor SAND = new MapColor(2, 16247203);
    public static readonly MapColor WOOL = new MapColor(3, 13092807);
    public static readonly MapColor FIRE = new MapColor(4, 16711680);
    public static readonly MapColor ICE = new MapColor(5, 10526975);
    public static readonly MapColor METAL = new MapColor(6, 10987431);
    public static readonly MapColor PLANT = new MapColor(7, 31744);
    public static readonly MapColor SNOW = new MapColor(8, 16777215);
    public static readonly MapColor CLAY = new MapColor(9, 10791096);
    public static readonly MapColor DIRT = new MapColor(10, 9923917);
    public static readonly MapColor STONE = new MapColor(11, 7368816);
    public static readonly MapColor WATER = new MapColor(12, 4210943);
    public static readonly MapColor WOOD = new MapColor(13, 9402184);
    public static readonly MapColor QUARTZ = new MapColor(14, 16776437);
    public static readonly MapColor COLOR_ORANGE = new MapColor(15, 14188339);
    public static readonly MapColor COLOR_MAGENTA = new MapColor(16, 11685080);
    public static readonly MapColor COLOR_LIGHT_BLUE = new MapColor(17, 6724056);
    public static readonly MapColor COLOR_YELLOW = new MapColor(18, 15066419);
    public static readonly MapColor COLOR_LIGHT_GREEN = new MapColor(19, 8375321);
    public static readonly MapColor COLOR_PINK = new MapColor(20, 15892389);
    public static readonly MapColor COLOR_GRAY = new MapColor(21, 5000268);
    public static readonly MapColor COLOR_LIGHT_GRAY = new MapColor(22, 10066329);
    public static readonly MapColor COLOR_CYAN = new MapColor(23, 5013401);
    public static readonly MapColor COLOR_PURPLE = new MapColor(24, 8339378);
    public static readonly MapColor COLOR_BLUE = new MapColor(25, 3361970);
    public static readonly MapColor COLOR_BROWN = new MapColor(26, 6704179);
    public static readonly MapColor COLOR_GREEN = new MapColor(27, 6717235);
    public static readonly MapColor COLOR_RED = new MapColor(28, 10040115);
    public static readonly MapColor COLOR_BLACK = new MapColor(29, 1644825);
    public static readonly MapColor GOLD = new MapColor(30, 16445005);
    public static readonly MapColor DIAMOND = new MapColor(31, 6085589);
    public static readonly MapColor LAPIS = new MapColor(32, 4882687);
    public static readonly MapColor EMERALD = new MapColor(33, 55610);
    public static readonly MapColor PODZOL = new MapColor(34, 8476209);
    public static readonly MapColor NETHER = new MapColor(35, 7340544);
    public static readonly MapColor TERRACOTTA_WHITE = new MapColor(36, 13742497);
    public static readonly MapColor TERRACOTTA_ORANGE = new MapColor(37, 10441252);
    public static readonly MapColor TERRACOTTA_MAGENTA = new MapColor(38, 9787244);
    public static readonly MapColor TERRACOTTA_LIGHT_BLUE = new MapColor(39, 7367818);
    public static readonly MapColor TERRACOTTA_YELLOW = new MapColor(40, 12223780);
    public static readonly MapColor TERRACOTTA_LIGHT_GREEN = new MapColor(41, 6780213);
    public static readonly MapColor TERRACOTTA_PINK = new MapColor(42, 10505550);
    public static readonly MapColor TERRACOTTA_GRAY = new MapColor(43, 3746083);
    public static readonly MapColor TERRACOTTA_LIGHT_GRAY = new MapColor(44, 8874850);
    public static readonly MapColor TERRACOTTA_CYAN = new MapColor(45, 5725276);
    public static readonly MapColor TERRACOTTA_PURPLE = new MapColor(46, 8014168);
    public static readonly MapColor TERRACOTTA_BLUE = new MapColor(47, 4996700);
    public static readonly MapColor TERRACOTTA_BROWN = new MapColor(48, 4993571);
    public static readonly MapColor TERRACOTTA_GREEN = new MapColor(49, 5001770);
    public static readonly MapColor TERRACOTTA_RED = new MapColor(50, 9321518);
    public static readonly MapColor TERRACOTTA_BLACK = new MapColor(51, 2430480);
    public static readonly MapColor CRIMSON_NYLIUM = new MapColor(52, 12398641);
    public static readonly MapColor CRIMSON_STEM = new MapColor(53, 9715553);
    public static readonly MapColor CRIMSON_HYPHAE = new MapColor(54, 6035741);
    public static readonly MapColor WARPED_NYLIUM = new MapColor(55, 1474182);
    public static readonly MapColor WARPED_STEM = new MapColor(56, 3837580);
    public static readonly MapColor WARPED_HYPHAE = new MapColor(57, 5647422);
    public static readonly MapColor WARPED_WART_BLOCK = new MapColor(58, 1356933);
    public static readonly MapColor DEEPSLATE = new MapColor(59, 6579300);
    public static readonly MapColor RAW_IRON = new MapColor(60, 14200723);
    public static readonly MapColor GLOW_LICHEN = new MapColor(61, 8365974);
    public int Id { get; }
    public int Color { get; }

    private MapColor(int id, int color)
    {
        this.Id = id;
        this.Color = color;
        Colors[id] = this;
    }

    private static MapColor byIdUnsafe(int a)
    {
        MapColor? col = Colors[a];
        if (col is null)
        {
            return MapColor.NONE;
        }

        return col;
    }

    public int calculateRGBColor(MapColor.Brightness a)
    {
        if (this == NONE)
        {
            return 0;
        }

        int b = a.Modifier;
        int c = (this.Color >> 16 & 255) * b / 255;
        int d = (this.Color >> 8 & 255) * b / 255;
        int e = (this.Color & 255) * b / 255;
        return -16777216 | e << 16 | d << 8 | c;
    }

    public class Brightness
    {
        public static Brightness LOW = new Brightness(0, 180);
        public static Brightness NORMAL = new Brightness(1, 220);
        public static Brightness HIGH = new Brightness(2, 255);
        public static Brightness LOWEST = new Brightness(3, 135);

        private static Brightness[] VALUES = new Brightness[] { LOW, NORMAL, HIGH, LOWEST };
        public int Modifier { get; }
        public int Id { get; }

        private Brightness(int id, int modifier)
        {
            Modifier = modifier;
            Id = id;
        }


        public static Brightness byIdUnsafe(int a)
        {
            return VALUES[a];
        }
    }

    public static int getColorFromPackedId(int a)
    {
        int b = a & 255;
        return byIdUnsafe(b >> 2).calculateRGBColor(MapColor.Brightness.byIdUnsafe(b & 3));
    }
}