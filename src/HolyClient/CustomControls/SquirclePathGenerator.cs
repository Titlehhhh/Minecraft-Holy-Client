using Avalonia.Media;

namespace HolyClient.CustomControls;

internal static class SquirclePathGenerator
{
    public static PathGeometry GetGeometry(double width = 100, double height = 100, double curvature = 1)
    {
        return PathGeometry.Parse(SquircleGenerator.GetGeometry(width, height, curvature));
    }
}