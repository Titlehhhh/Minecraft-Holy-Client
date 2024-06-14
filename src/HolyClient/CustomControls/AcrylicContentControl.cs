using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace HolyClient.CustomControls;

public class AcrylicContentControl : ContentControl
{
    private static readonly ImmutableExperimentalAcrylicMaterial DefaultAcrylicMaterial =
        (ImmutableExperimentalAcrylicMaterial)new ExperimentalAcrylicMaterial
        {
            MaterialOpacity = 0.5,
            TintColor = new Color(255, 7, 7, 7),
            TintOpacity = 1,
            PlatformTransparencyCompensationLevel = 0
        }.ToImmutable();

    public static readonly StyledProperty<ExperimentalAcrylicMaterial?> MaterialProperty =
        AvaloniaProperty.Register<AcrylicContentControl, ExperimentalAcrylicMaterial?>(nameof(Material));

    public static readonly StyledProperty<int> BlurProperty =
        AvaloniaProperty.Register<AcrylicContentControl, int>(nameof(Blur));

    static AcrylicContentControl()
    {
        AffectsRender<AcrylicContentControl>(MaterialProperty);
        AffectsRender<AcrylicContentControl>(BlurProperty);
    }

    public ExperimentalAcrylicMaterial? Material
    {
        get => GetValue(MaterialProperty);
        set => SetValue(MaterialProperty, value);
    }

    public int Blur
    {
        get => GetValue(BlurProperty);
        set => SetValue(BlurProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        var mat = Material != null
            ? (ImmutableExperimentalAcrylicMaterial)Material.ToImmutable()
            : DefaultAcrylicMaterial;
        context.Custom(new AcrylicContentControlRenderOperation(this, mat, Blur, new Rect(default, Bounds.Size)));
    }
}