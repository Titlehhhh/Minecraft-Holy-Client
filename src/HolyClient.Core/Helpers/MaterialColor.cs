using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.Core.Helpers
{
    public class MaterialColor
    {
        public static Dictionary<int, MaterialColor> MATERIAL_COLORS = new();
        public static MaterialColor NONE = new MaterialColor(0, 0);
        public static MaterialColor GRASS = new MaterialColor(1, 8368696);
        public static MaterialColor SAND = new MaterialColor(2, 16247203);
        public static MaterialColor WOOL = new MaterialColor(3, 13092807);
        public static MaterialColor FIRE = new MaterialColor(4, 16711680);
        public static MaterialColor ICE = new MaterialColor(5, 10526975);
        public static MaterialColor METAL = new MaterialColor(6, 10987431);
        public static MaterialColor PLANT = new MaterialColor(7, 31744);
        public static MaterialColor SNOW = new MaterialColor(8, 16777215);
        public static MaterialColor CLAY = new MaterialColor(9, 10791096);
        public static MaterialColor DIRT = new MaterialColor(10, 9923917);
        public static MaterialColor STONE = new MaterialColor(11, 7368816);
        public static MaterialColor WATER = new MaterialColor(12, 4210943);
        public static MaterialColor WOOD = new MaterialColor(13, 9402184);
        public static MaterialColor QUARTZ = new MaterialColor(14, 16776437);
        public static MaterialColor COLOR_ORANGE = new MaterialColor(15, 14188339);
        public static MaterialColor COLOR_MAGENTA = new MaterialColor(16, 11685080);
        public static MaterialColor COLOR_LIGHT_BLUE = new MaterialColor(17, 6724056);
        public static MaterialColor COLOR_YELLOW = new MaterialColor(18, 15066419);
        public static MaterialColor COLOR_LIGHT_GREEN = new MaterialColor(19, 8375321);
        public static MaterialColor COLOR_PINK = new MaterialColor(20, 15892389);
        public static MaterialColor COLOR_GRAY = new MaterialColor(21, 5000268);
        public static MaterialColor COLOR_LIGHT_GRAY = new MaterialColor(22, 10066329);
        public static MaterialColor COLOR_CYAN = new MaterialColor(23, 5013401);
        public static MaterialColor COLOR_PURPLE = new MaterialColor(24, 8339378);
        public static MaterialColor COLOR_BLUE = new MaterialColor(25, 3361970);
        public static MaterialColor COLOR_BROWN = new MaterialColor(26, 6704179);
        public static MaterialColor COLOR_GREEN = new MaterialColor(27, 6717235);
        public static MaterialColor COLOR_RED = new MaterialColor(28, 10040115);
        public static MaterialColor COLOR_BLACK = new MaterialColor(29, 1644825);
        public static MaterialColor GOLD = new MaterialColor(30, 16445005);
        public static MaterialColor DIAMOND = new MaterialColor(31, 6085589);
        public static MaterialColor LAPIS = new MaterialColor(32, 4882687);
        public static MaterialColor EMERALD = new MaterialColor(33, 55610);
        public static MaterialColor PODZOL = new MaterialColor(34, 8476209);
        public static MaterialColor NETHER = new MaterialColor(35, 7340544);
        public static MaterialColor TERRACOTTA_WHITE = new MaterialColor(36, 13742497);
        public static MaterialColor TERRACOTTA_ORANGE = new MaterialColor(37, 10441252);
        public static MaterialColor TERRACOTTA_MAGENTA = new MaterialColor(38, 9787244);
        public static MaterialColor TERRACOTTA_LIGHT_BLUE = new MaterialColor(39, 7367818);
        public static MaterialColor TERRACOTTA_YELLOW = new MaterialColor(40, 12223780);
        public static MaterialColor TERRACOTTA_LIGHT_GREEN = new MaterialColor(41, 6780213);
        public static MaterialColor TERRACOTTA_PINK = new MaterialColor(42, 10505550);
        public static MaterialColor TERRACOTTA_GRAY = new MaterialColor(43, 3746083);
        public static MaterialColor TERRACOTTA_LIGHT_GRAY = new MaterialColor(44, 8874850);
        public static MaterialColor TERRACOTTA_CYAN = new MaterialColor(45, 5725276);
        public static MaterialColor TERRACOTTA_PURPLE = new MaterialColor(46, 8014168);
        public static MaterialColor TERRACOTTA_BLUE = new MaterialColor(47, 4996700);
        public static MaterialColor TERRACOTTA_BROWN = new MaterialColor(48, 4993571);
        public static MaterialColor TERRACOTTA_GREEN = new MaterialColor(49, 5001770);
        public static MaterialColor TERRACOTTA_RED = new MaterialColor(50, 9321518);
        public static MaterialColor TERRACOTTA_BLACK = new MaterialColor(51, 2430480);
        public static MaterialColor CRIMSON_NYLIUM = new MaterialColor(52, 12398641);
        public static MaterialColor CRIMSON_STEM = new MaterialColor(53, 9715553);
        public static MaterialColor CRIMSON_HYPHAE = new MaterialColor(54, 6035741);
        public static MaterialColor WARPED_NYLIUM = new MaterialColor(55, 1474182);
        public static MaterialColor WARPED_STEM = new MaterialColor(56, 3837580);
        public static MaterialColor WARPED_HYPHAE = new MaterialColor(57, 5647422);
        public static MaterialColor WARPED_WART_BLOCK = new MaterialColor(58, 1356933);
        public int col;
        public int id;



        private MaterialColor(int n, int n2)
        {
            if (n < 0 || n > 63)
            {
                throw new Exception("Map colour ID must be between 0 and 63 (inclusive)");
            }
            col = n2;

            MATERIAL_COLORS.Add(n, this);
        }

        public int calculateRGBColor(int n)
        {
            int n2 = 220;
            if (n == 3)
            {
                n2 = 135;
            }
            if (n == 2)
            {
                n2 = 255;
            }
            if (n == 1)
            {
                n2 = 220;
            }
            if (n == 0)
            {
                n2 = 180;
            }
            int n3 = (col >> 16 & 255) * n2 / 255;
            int n4 = (col >> 8 & 0xFF) * n2 / 255;
            int n5 = (col & 0xFF) * n2 / 255;

            return (int)(4278190080 | n5 << 16 | n4 << 8 | n3);

        }
    }
}
