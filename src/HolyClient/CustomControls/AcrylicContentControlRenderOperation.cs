using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;

namespace HolyClient.CustomControls;

public class AcrylicContentControlRenderOperation : ICustomDrawOperation
{
    private static SKShader? _acrylicNoiseShader;

    private readonly AcrylicContentControl _AcrylicContentControl;
    private readonly int _blur;
    private readonly Rect _bounds;
    private readonly CornerRadius _cornerRadius;
    private readonly ImmutableExperimentalAcrylicMaterial _material;
    private SKImage? _backgroundSnapshot;
    private bool _disposed;

    public AcrylicContentControlRenderOperation(AcrylicContentControl AcrylicContentControl,
        ImmutableExperimentalAcrylicMaterial material, int blur, Rect bounds)
    {
        _AcrylicContentControl = AcrylicContentControl;
        _cornerRadius = AcrylicContentControl.CornerRadius;
        _material = material;
        _blur = blur;
        _bounds = bounds;
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _backgroundSnapshot?.Dispose();
        _disposed = true;
    }

    public bool HitTest(Point p)
    {
        return _bounds.Contains(p);
    }

    public void Render(ImmediateDrawingContext context)
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(AcrylicContentControlRenderOperation));

        var leaseFeature = context.PlatformImpl.GetFeature<ISkiaSharpApiLeaseFeature>();
        if (leaseFeature == null)
            return;
        using var lease = leaseFeature.Lease();

        if (!lease.SkCanvas.TotalMatrix.TryInvert(out var currentInvertedTransform) || lease.SkSurface == null)
            return;

        if (lease.SkCanvas.GetLocalClipBounds(out var bounds) && !bounds.Contains(SKRect.Create(bounds.Left, bounds.Top,
                (float)_AcrylicContentControl.Bounds.Width, (float)_AcrylicContentControl.Bounds.Height)))
        {
            //Dispatcher.UIThread.Invoke(() => _AcrylicContentControl.InvalidateVisual());
        }
        else
        {
            _backgroundSnapshot?.Dispose();
            _backgroundSnapshot = lease.SkSurface.Snapshot();
        }

        _backgroundSnapshot ??= lease.SkSurface.Snapshot();
        using var backdropShader = SKShader.CreateImage(_backgroundSnapshot, SKShaderTileMode.Clamp,
            SKShaderTileMode.Clamp, currentInvertedTransform);
        using var blurred = SKSurface.Create(
            lease.GrContext,
            false,
            new SKImageInfo((int)Math.Ceiling(_bounds.Width), (int)Math.Ceiling(_bounds.Height),
                SKImageInfo.PlatformColorType, SKAlphaType.Premul)
        );
        using (var filter = SKImageFilter.CreateBlur(_blur, _blur, SKShaderTileMode.Clamp))
        using (var blurPaint = new SKPaint { Shader = backdropShader, ImageFilter = filter })
        {
            blurred.Canvas.DrawRoundRect(0, 0, (float)_bounds.Width, (float)_bounds.Height, 10, 10, blurPaint);

            using (var blurSnap = blurred.Snapshot())
            using (var blurSnapShader = SKShader.CreateImage(blurSnap))
            using (var blurSnapPaint = new SKPaint { Shader = blurSnapShader, IsAntialias = true })
            {
                // Rendering twice to reduce opacity
                lease.SkCanvas.DrawRoundRect(
                    0,
                    0,
                    (float)_bounds.Width,
                    (float)_bounds.Height,
                    10, 10,
                    blurSnapPaint);

                //lease.SkCanvas.DrawRoundRect(
                //	0, 
                //	0,
                //	(float)_bounds.Width,
                //	(float)_bounds.Height,
                //	10,10,
                //	blurSnapPaint);
            }

            //return;
            using var acrylliPaint = new SKPaint();
            acrylliPaint.IsAntialias = true;

            double opacity = 1;

            const double noiseOpacity = 0.0125;

            var tintColor = _material.TintColor;
            var tint = new SKColor(tintColor.R, tintColor.G, tintColor.B, tintColor.A);

            if (_acrylicNoiseShader == null)
            {
                using var stream =
                    typeof(SkiaPlatform).Assembly.GetManifestResourceStream(
                        "Avalonia.Skia.Assets.NoiseAsset_256X256_PNG.png");
                using var bitmap = SKBitmap.Decode(stream);
                _acrylicNoiseShader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat)
                    .WithColorFilter(CreateAlphaColorFilter(noiseOpacity));
            }

            using (var backdrop = SKShader.CreateColor(new SKColor(_material.MaterialColor.R, _material.MaterialColor.G,
                       _material.MaterialColor.B, _material.MaterialColor.A)))
            using (var tintShader = SKShader.CreateColor(tint))
            using (var effectiveTint = SKShader.CreateCompose(backdrop, tintShader))
            using (var compose = SKShader.CreateCompose(effectiveTint, _acrylicNoiseShader))
            {
                acrylliPaint.Shader = compose;
                acrylliPaint.IsAntialias = true;
                lease.SkCanvas.DrawRoundRect(
                    0,
                    0,
                    (float)_bounds.Width,
                    (float)_bounds.Height,
                    10,
                    10,
                    acrylliPaint);
            }
        }
    }

    public Rect Bounds => _bounds.Inflate(4);

    public bool Equals(ICustomDrawOperation? other)
    {
        return other is AcrylicContentControlRenderOperation op && op._bounds == _bounds &&
               op._material.Equals(_material);
    }

    private static SKColorFilter CreateAlphaColorFilter(double opacity)
    {
        if (opacity > 1)
            opacity = 1;
        var c = new byte[256];
        var a = new byte[256];
        for (var i = 0; i < 256; i++)
        {
            c[i] = (byte)i;
            a[i] = (byte)(i * opacity);
        }

        return SKColorFilter.CreateTable(a, c, c, c);
    }
}