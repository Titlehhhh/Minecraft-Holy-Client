using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace HolyClient.CustomControls
{

	public class Squircle : Decorator
	{
		#region Private Fields

		private PathGeometry? _borderGeometryCache;

		private Pen? _penCache;

		#endregion

		#region Dependency Properties

		/// <summary>
		/// DependencyProperty for <see cref="Background" /> property.
		/// </summary>
		public static readonly StyledProperty<IBrush?> BackgroundProperty =
			Panel.BackgroundProperty.AddOwner<Squircle>();

		public static readonly StyledProperty<double> CurvatureProperty =
			AvaloniaProperty.Register<Squircle, double>(nameof(Curvature), 1.0, validate: IsCurvatureValid);

		private static bool IsCurvatureValid(double curvature) => curvature >= 0 && curvature <= 1;

		/// <summary>
		/// DependencyProperty for <see cref="BorderThickness" /> property.
		/// </summary>
		public static readonly StyledProperty<double> BorderThicknessProperty =
			AvaloniaProperty.Register<Squircle, double>(nameof(BorderThickness), 0.0,
				validate: IsBorderThicknessValid);


		private static bool IsBorderThicknessValid(double borderThickness) => borderThickness >= 0;

		private static void OnClearPenCache(AvaloniaPropertyChangedEventArgs<double> args)
		{

			(args.Sender as Squircle)._penCache = null;
		}
		private static void OnClearPenCache(AvaloniaPropertyChangedEventArgs<IBrush?> args)
		{

			(args.Sender as Squircle)._penCache = null;
		}

		/// <summary>
		/// DependencyProperty for <see cref="BorderBrush" /> property.
		/// </summary>
		public static readonly StyledProperty<IBrush?> BorderBrushProperty =
			AvaloniaProperty.Register<Squircle, IBrush?>(nameof(BorderBrush), default);

		#endregion

		#region Public Properties

		public double Curvature
		{
			get => GetValue(CurvatureProperty);
			set => SetValue(CurvatureProperty, value);
		}

		public double BorderThickness
		{
			get => GetValue(BorderThicknessProperty);
			set => SetValue(BorderThicknessProperty, value);
		}

		/// <summary>
		/// The BorderBrush property defines the brush used to fill the border region.
		/// </summary>
		public IBrush? BorderBrush
		{
			get => GetValue(BorderBrushProperty);
			set => SetValue(BorderBrushProperty, value);
		}

		/// <summary>
		/// The Background property defines the brush used to fill the area within the border.
		/// </summary>
		public IBrush? Background
		{
			get => GetValue(BackgroundProperty);
			set => SetValue(BackgroundProperty, value);
		}

		#endregion

		#region Static Constructor

		static Squircle()
		{
			BorderThicknessProperty.Changed
				.Subscribe(OnClearPenCache);

			BorderBrushProperty.Changed
				.Subscribe(OnClearPenCache);

			AffectsMeasure<Squircle>(CurvatureProperty, BorderThicknessProperty, PaddingProperty);
			AffectsRender<Squircle>(CurvatureProperty, BorderThicknessProperty, PaddingProperty, BorderBrushProperty,
				BackgroundProperty);
		}

		#endregion

		#region Public Methods

		public override void Render(DrawingContext context)
		{
			if (_borderGeometryCache != null
				&& Background != null)
			{
				context.DrawGeometry(Background, null, _borderGeometryCache);
			}

			if (_borderGeometryCache != null && BorderBrush != null)
			{
				var pen = _penCache;
				if (pen == null)
				{
					pen = new Pen
					{
						Brush = BorderBrush,
						Thickness = BorderThickness,
						LineJoin = PenLineJoin.Round
					};

					_penCache = pen;
				}

				context.DrawGeometry(null, _penCache, _borderGeometryCache);
			}
		}

		#endregion

		#region Protected Methods

		protected override Size MeasureOverride(Size availableSize)
		{
			var child = Child;

			var borderThicknessSize = HelperCollapseThickness(new Thickness(BorderThickness));
			var paddingSize = HelperCollapseThickness(this.Padding);

			var mySize = new Size(borderThicknessSize.Width + paddingSize.Width,
				borderThicknessSize.Height + paddingSize.Height);

			if (child != null)
			{
				var childConstraint = new Size(Math.Max(0.0, availableSize.Width - mySize.Width),
					Math.Max(0.0, availableSize.Height - mySize.Height));

				child.Measure(childConstraint);
				var childSize = child.DesiredSize;

				mySize = new Size(childSize.Width + mySize.Width, childSize.Height + mySize.Height);
			}

			return mySize;
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			Rect boundRect = new(finalSize);
			var innerRect = HelperDeflateRect(boundRect, new Thickness(BorderThickness));

			Control child = Child;
			if (child != null)
			{
				var childRect = HelperDeflateRect(innerRect, Padding);
				child.Arrange(childRect);
			}

			if (innerRect.Width != 0 && innerRect.Height != 0)
			{
				PathGeometry borderGeometry =
					SquirclePathGenerator.GetGeometry(boundRect.Width - BorderThickness,
						boundRect.Height - BorderThickness, Curvature);

				borderGeometry.Transform = new TranslateTransform(BorderThickness / 2, BorderThickness / 2);

				_borderGeometryCache = borderGeometry;
			}

			return finalSize;
		}

		#endregion

		#region Private Methods

		// Helper function to add up the left and right size as width, as well as the top and bottom size as height
		private static Size HelperCollapseThickness(Thickness th)
			=> new(th.Left + th.Right, th.Top + th.Bottom);

		/// Helper to deflate rectangle by thickness
		private static Rect HelperDeflateRect(Rect rt, Thickness thick)
		{
			return new(rt.Left + thick.Left,
				rt.Top + thick.Top,
				Math.Max(0.0, rt.Width - thick.Left - thick.Right),
				Math.Max(0.0, rt.Height - thick.Top - thick.Bottom));
		}

		#endregion Private Methods
	}

}
