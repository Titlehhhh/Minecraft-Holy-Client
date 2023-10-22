using System;
using System.Globalization;
using System.Text;

namespace HolyClient.CustomControls
{
	public static class SquircleGenerator
	{
		public static string GetGeometry(double width = 100, double height = 100, double curvature = 1)
		{
			StringBuilder path = new();

			var halfHeight = height / 2;
			var halfWidth = width / 2;

			var arc = Math.Min(halfWidth, halfHeight) * (1 - curvature);

			path.Append(GetStartPoint(0, halfHeight))
				.Append(GetBezierSegment(0, arc, arc, 0, halfWidth, 0))
				.Append(GetShortBezierSegment(width, arc, width, halfHeight))
				.Append(GetShortBezierSegment(width - arc, height, halfWidth, height))
				.Append(GetShortBezierSegment(0, height - arc, 0, halfHeight))
				.Append(" Z");
			string path_s = path.ToString();
			return path_s;
		}

		private static string GetStartPoint(double x, double y)
			=> string.Format(CultureInfo.InvariantCulture, "M {0}, {1}", x, y);

		private static string GetShortBezierSegment(double x1, double y1, double x, double y)
			=> string.Format(CultureInfo.InvariantCulture, "S {0}, {1} {2}, {3}", x1, y1, x, y);

		private static string GetBezierSegment(double x1, double y1, double x2, double y2, double x, double y)
			=> string.Format(CultureInfo.InvariantCulture, "C {0}, {1} {2}, {3} {4}, {5}", x1, y1, x2, y2, x, y);
	}

}
