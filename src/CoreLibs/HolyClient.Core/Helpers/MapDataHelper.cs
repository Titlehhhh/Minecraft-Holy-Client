using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace HolyClient.Core.Helpers;

public class MapDataHelper
{
    public static Image<Rgba32> CreateImage(byte[] colors)
    {
        Image<Rgba32> image = new(128, 128);

        for (var x = 0; x < 128; ++x)
        for (var y = 0; y < 128; ++y)
        {
            var n = y + x * 128;
            var n2 = colors[n] & 255;
            if (n2 / 4 == 4) image[x, y] = new Rgba32(0, 0, 0, 0);

            var rgb = MaterialColor.MATERIAL_COLORS[n2 / 4].calculateRGBColor(n2 & 3);

            var values = BitConverter.GetBytes(rgb);

            if (!BitConverter.IsLittleEndian)
                Array.Reverse(values);

            var color = new Rgba32(values[0], values[1], values[2]);


            image[y, x] = color;
        }

        return image;
    }
}