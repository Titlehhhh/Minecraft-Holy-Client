namespace HolyClient.Core.Helpers
{
	public class MapDataHelper
	{
		public static Image<Rgba32> CreateImage(byte[] colors)
		{

			Image<Rgba32> image = new(128, 128);

			for (int x = 0; x < 128; ++x)
			{
				for (int y = 0; y < 128; ++y)
				{
					int n = y + x * 128;
					int n2 = colors[n] & 255;
					if (n2 / 4 == 4)
					{
						image[x, y] = new Rgba32(0, 0, 0, 0);
					}

					int rgb = MaterialColor.MATERIAL_COLORS[n2 / 4].calculateRGBColor(n2 & 3);

					byte[] values = BitConverter.GetBytes(rgb);

					if (!BitConverter.IsLittleEndian)
						Array.Reverse(values);

					var color = new Rgba32(values[0], values[1], values[2]);

					
					image[y, x] = color;
				}
			}
			return image;
		}
	}
}
