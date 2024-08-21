using Avalonia.Controls.Primitives;

namespace HolyClient.CustomControls;

public class LoadNugetAnimationControl : TemplatedControl
{
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        //double maxSideLength = Math.Min(this.Width, this.Height);
        //double ellipseDiameter = 0.1 * maxSideLength;
        //if (maxSideLength <= 40)
        //{
        //	ellipseDiameter += 1;
        //}

        //EllipseDiameter = ellipseDiameter;
        //MaxSideLength = maxSideLength;
        //EllipseOffset = new Thickness(0, maxSideLength / 2 - ellipseDiameter, 0, 0);
    }
}