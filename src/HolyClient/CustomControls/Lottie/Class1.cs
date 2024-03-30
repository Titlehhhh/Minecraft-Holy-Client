using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Platform;
using Avalonia.Rendering.Composition;
using Avalonia.Skia;
using SkiaSharp;
using System;
using System.IO;
using System.Numerics;

namespace HolyClient.CustomControls
{
	public class Lottie : Control
	{
		private SkiaSharp.Skottie.Animation? _animation;
		private int _repeatCount;
		private readonly Uri _baseUri;
		private CompositionCustomVisual? _customVisual;

		/// <summary>
		/// Infinite number of repeats.
		/// </summary>
		public const int Infinity = -1;

		/// <summary>
		/// Defines the <see cref="Path"/> property.
		/// </summary>
		public static readonly StyledProperty<string?> PathProperty =
			AvaloniaProperty.Register<Lottie, string?>(nameof(Path));

		/// <summary>
		/// Defines the <see cref="Stretch"/> property.
		/// </summary>
		public static readonly StyledProperty<Stretch> StretchProperty =
			AvaloniaProperty.Register<Lottie, Stretch>(nameof(Stretch), Stretch.Uniform);

		/// <summary>Lottie
		/// Defines the <see cref="StretchDirection"/> property.
		/// </summary>
		public static readonly StyledProperty<StretchDirection> StretchDirectionProperty =
			AvaloniaProperty.Register<Lottie, StretchDirection>(
				nameof(StretchDirection),
				StretchDirection.Both);

		/// <summary>
		/// Defines the <see cref="RepeatCount"/> property.
		/// </summary>
		public static readonly StyledProperty<int> RepeatCountProperty =
			AvaloniaProperty.Register<Lottie, int>(nameof(RepeatCount), Infinity);

		/// <summary>
		/// Gets or sets the Lottie animation path.
		/// </summary>
		[Content]
		public string? Path
		{
			get => GetValue(PathProperty);
			set => SetValue(PathProperty, value);
		}

		/// <summary>
		/// Gets or sets a value controlling how the image will be stretched.
		/// </summary>
		public Stretch Stretch
		{
			get { return GetValue(StretchProperty); }
			set { SetValue(StretchProperty, value); }
		}

		/// <summary>
		/// Gets or sets a value controlling in what direction the image will be stretched.
		/// </summary>
		public StretchDirection StretchDirection
		{
			get { return GetValue(StretchDirectionProperty); }
			set { SetValue(StretchDirectionProperty, value); }
		}

		/// <summary>
		///  Sets how many times the animation should be repeated.
		/// </summary>
		public int RepeatCount
		{
			get => GetValue(RepeatCountProperty);
			set => SetValue(RepeatCountProperty, value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Lottie"/> class.
		/// </summary>
		/// <param name="baseUri">The base URL for the XAML context.</param>
		public Lottie(Uri baseUri)
		{
			_baseUri = baseUri;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Lottie"/> class.
		/// </summary>
		/// <param name="serviceProvider">The XAML service provider.</param>
		public Lottie(IServiceProvider serviceProvider)
		{
			_baseUri = serviceProvider.GetContextBaseUri();
		}

		/// <inheritdoc/>
		protected override void OnLoaded(RoutedEventArgs e)
		{
			base.OnLoaded(e);

			var elemVisual = ElementComposition.GetElementVisual(this);
			var compositor = elemVisual?.Compositor;
			if (compositor is null)
			{
				return;
			}

			_customVisual = compositor.CreateCustomVisual(new LottieCompositionCustomVisualHandler());
			ElementComposition.SetElementChildVisual(this, _customVisual);

			LayoutUpdated += OnLayoutUpdated;

			string? path = Path;
			if (path is null)
			{
				return;
			}

			DisposeImpl();
			Load(path);

			_customVisual.Size = new Vector2((float)Bounds.Size.Width, (float)Bounds.Size.Height);
			_customVisual.SendHandlerMessage(
				new LottiePayload(
					LottieCommand.Update,
					_animation,
					Stretch,
					StretchDirection));

			Start();
		}

		protected override void OnUnloaded(RoutedEventArgs e)
		{
			base.OnUnloaded(e);

			LayoutUpdated -= OnLayoutUpdated;

			Stop();
			DisposeImpl();
			_animation = null;
			_customVisual = null;
		}

		private void OnLayoutUpdated(object? sender, EventArgs e)
		{
			if (_customVisual == null)
			{
				return;
			}

			_customVisual.Size = new Vector2((float)Bounds.Size.Width, (float)Bounds.Size.Height);
			_customVisual.SendHandlerMessage(
				new LottiePayload(
					LottieCommand.Update,
					_animation,
					Stretch,
					StretchDirection));
		}

		/// <inheritdoc/>
		protected override Size MeasureOverride(Size availableSize)
		{
			if (_animation == null)
			{
				return new Size();
			}

			var sourceSize = _animation is { }
				? new Size(_animation.Size.Width, _animation.Size.Height)
				: default;

			return Stretch.CalculateSize(availableSize, sourceSize, StretchDirection);
		}

		protected override Size ArrangeOverride(Size finalSize)
		{
			if (_animation == null)
			{
				return new Size();
			}

			var sourceSize = _animation is { }
				? new Size(_animation.Size.Width, _animation.Size.Height)
				: default;

			return Stretch.CalculateSize(finalSize, sourceSize);
		}

		/// <inheritdoc/>
		protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
		{
			base.OnPropertyChanged(change);

			switch (change.Property.Name)
			{
				case nameof(Path):
					{
						if (_customVisual is null)
						{
							return;
						}

						var path = change.GetNewValue<string?>();
						Load(path);
						break;
					}
				case nameof(RepeatCount):
					{
						_repeatCount = change.GetNewValue<int>();
						Stop();
						Start();
						break;
					}
			}
		}

		private SkiaSharp.Skottie.Animation? Load(Stream stream)
		{

			//using var managedStream = new SKManagedStream(stream);
			using (StreamReader sr = new StreamReader(stream))
			{
				var json = sr.ReadToEnd();
				return SkiaSharp.Skottie.Animation.Parse(json);
			}

			//if (SkiaSharp.Skottie.Animation.TryCreate(managedStream, out var animation))
			//{
			//	animation.Seek(0);

			//	//Logger
			//	//	.TryGet(LogEventLevel.Information, LogArea.Control)?
			//	//	.Log(this, $"Version: {animation.Version} Duration: {animation.Duration} Fps:{animation.Fps} InPoint: {animation.InPoint} OutPoint: {animation.OutPoint}");
			//}
			//else
			//{

			//	//Logger
			//	//	.TryGet(LogEventLevel.Warning, LogArea.Control)?
			//	//	.Log(this, "Failed to load animation.");
			//}

			//return animation;
		}

		private SkiaSharp.Skottie.Animation? Load(string path, Uri? baseUri)
		{
			var uri = path.StartsWith("/")
				? new Uri(path, UriKind.Relative)
				: new Uri(path, UriKind.RelativeOrAbsolute);
			if (uri.IsAbsoluteUri && uri.IsFile)
			{
				using var fileStream = File.OpenRead(uri.LocalPath);
				return Load(fileStream);
			}

			using var assetStream = AssetLoader.Open(uri, baseUri);

			if (assetStream is null)
			{
				return default;
			}

			return Load(assetStream);
		}

		private void Load(string? path)
		{
			Stop();

			if (path is null)
			{
				DisposeImpl();
				return;
			}

			DisposeImpl();

			try
			{
				_repeatCount = RepeatCount;
				_animation = Load(path, _baseUri);

				if (_animation is null)
				{
					return;
				}

				InvalidateArrange();
				InvalidateMeasure();

				Start();
			}
			catch (Exception e)
			{
				//Logger
				//	.TryGet(LogEventLevel.Warning, LogArea.Control)?
				//	.Log(this, "Failed to load animation: " + e);
				_animation = null;
			}
		}

		private void Start()
		{
			_customVisual?.SendHandlerMessage(
				new LottiePayload(
					LottieCommand.Start,
					_animation,
					Stretch,
					StretchDirection,
					_repeatCount));
		}

		private void Stop()
		{
			_customVisual?.SendHandlerMessage(new LottiePayload(LottieCommand.Stop));
		}

		private void DisposeImpl()
		{
			_customVisual?.SendHandlerMessage(new LottiePayload(LottieCommand.Dispose));
		}
	}
	internal static class ServiceProviderExtensions
	{
		public static T GetService<T>(this IServiceProvider sp)
			=> (T)sp?.GetService(typeof(T))!;

		public static Uri GetContextBaseUri(this IServiceProvider ctx)
			=> ctx.GetService<IUriContext>().BaseUri;
	}
	internal enum LottieCommand
	{
		Start,
		Stop,
		Update,
		Dispose
	}

	internal class LottieCompositionCustomVisualHandler : CompositionCustomVisualHandler
	{
		private TimeSpan _primaryTimeElapsed, _animationElapsed;
		private TimeSpan? _lastServerTime;
		private bool _running;
		private SkiaSharp.Skottie.Animation? _animation;
		private Stretch? _stretch;
		private StretchDirection? _stretchDirection;
		private SkiaSharp.SceneGraph.InvalidationController? _ic;
		private readonly object _sync = new();
		private int _repeatCount;
		private int _count;

		public override void OnMessage(object message)
		{
			if (message is not LottiePayload msg)
			{
				return;
			}

			switch (msg)
			{
				case
				{
					LottieCommand: LottieCommand.Start,
					Animation: { } an,
					Stretch: { } st,
					StretchDirection: { } sd,
					RepeatCount: { } rp
				}:
					{
						_running = true;
						_lastServerTime = null;
						_stretch = st;
						_stretchDirection = sd;
						_animation = an;
						_repeatCount = rp;
						_count = 0;
						_animationElapsed = TimeSpan.Zero;
						RegisterForNextAnimationFrameUpdate();
						break;
					}
				case
				{
					LottieCommand: LottieCommand.Update,
					Stretch: { } st,
					StretchDirection: { } sd
				}:
					{
						_stretch = st;
						_stretchDirection = sd;
						RegisterForNextAnimationFrameUpdate();
						break;
					}
				case
				{
					LottieCommand: LottieCommand.Stop
				}:
					{
						_running = false;
						_animationElapsed = TimeSpan.Zero;
						_count = 0;
						break;
					}
				case
				{
					LottieCommand: LottieCommand.Dispose
				}:
					{
						DisposeImpl();
						break;
					}
			}
		}

		public override void OnAnimationFrameUpdate()
		{
			if (!_running)
				return;


			if (_repeatCount == 0 || (_repeatCount > 0 && _count >= _repeatCount))
			{
				_running = false;
				_animationElapsed = TimeSpan.Zero;
			}

			Invalidate();
			RegisterForNextAnimationFrameUpdate();
		}

		private void DisposeImpl()
		{
			lock (_sync)
			{
				_animation?.Dispose();
				_animation = null;
				_ic?.End();
				_ic?.Dispose();
				_ic = null;
			}
		}

		private double GetFrameTime()
		{
			if (_animation is null)
			{
				return 0f;
			}

			var frameTime = _animationElapsed.TotalSeconds;

			if (_animationElapsed.TotalSeconds > _animation.Duration.TotalSeconds)
			{
				_animationElapsed = TimeSpan.Zero;
				_ic?.End();
				_ic?.Begin();
				_count++;
			}

			return frameTime;
		}

		private void Draw(SKCanvas canvas)
		{
			var animation = _animation;
			if (animation is null)
			{
				return;
			}

			if (_ic is null)
			{
				_ic = new SkiaSharp.SceneGraph.InvalidationController();
				_ic.Begin();
			}

			var ic = _ic;

			if (_repeatCount == 0)
			{
				return;
			}

			var t = GetFrameTime();
			if (!_running)
			{
				t = (float)animation.Duration.TotalSeconds;
			}

			var dst = new SKRect(0, 0, animation.Size.Width, animation.Size.Height);

			animation.SeekFrameTime(t, ic);
			canvas.Save();
			animation.Render(canvas, dst);
			canvas.Restore();

			ic.Reset();
		}

		public override void OnRender(ImmediateDrawingContext context)
		{
			lock (_sync)
			{
				if (_running)
				{
					if (_lastServerTime.HasValue)
					{
						var delta = (CompositionNow - _lastServerTime.Value);
						_primaryTimeElapsed += delta;
						_animationElapsed += delta;
					}

					_lastServerTime = CompositionNow;
				}

				if (_animation is not { } an
					|| _stretch is not { } st
					|| _stretchDirection is not { } sd)
				{
					return;
				}


				var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
				if (leaseFeature is null)
				{
					return;
				}

				var rb = GetRenderBounds();

				var viewPort = new Rect(rb.Size);
				var sourceSize = new Size(an.Size.Width, an.Size.Height);
				if (sourceSize.Width <= 0 || sourceSize.Height <= 0)
				{
					return;
				}

				var scale = st.CalculateScaling(rb.Size, sourceSize, sd);
				var scaledSize = sourceSize * scale;
				var destRect = viewPort
					.CenterRect(new Rect(scaledSize))
					.Intersect(viewPort);
				var sourceRect = new Rect(sourceSize)
					.CenterRect(new Rect(destRect.Size / scale));

				var bounds = SKRect.Create(new SKPoint(), an.Size);
				var scaleMatrix = Matrix.CreateScale(
					destRect.Width / sourceRect.Width,
					destRect.Height / sourceRect.Height);
				var translateMatrix = Matrix.CreateTranslation(
					-sourceRect.X + destRect.X - bounds.Top,
					-sourceRect.Y + destRect.Y - bounds.Left);

				using (context.PushClip(destRect))
				using (context.PushPostTransform(translateMatrix * scaleMatrix))
				{
					using var lease = leaseFeature.Lease();
					var canvas = lease?.SkCanvas;
					if (canvas is null)
					{
						return;
					}
					Draw(canvas);
				}
			}
		}
	}

	internal record struct LottiePayload(
	LottieCommand LottieCommand,
	SkiaSharp.Skottie.Animation? Animation = null,
	Stretch? Stretch = null,
	StretchDirection? StretchDirection = null,
	int? RepeatCount = null);
}
