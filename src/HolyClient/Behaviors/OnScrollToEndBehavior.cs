using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Xaml.Interactivity;
using System.Windows.Input;

namespace HolyClient.Behaviors
{
	public class OnScrollToEndBehavior : Behavior<ScrollViewer>
	{
		public static readonly DirectProperty<OnScrollToEndBehavior, ICommand> CommandProperty =
				AvaloniaProperty.RegisterDirect<OnScrollToEndBehavior, ICommand>(
		nameof(Command),
		o => o.Command,
		(o, v) => o.Command = v);

		private ICommand _command;

		public ICommand Command
		{
			get { return _command; }
			set { SetAndRaise(CommandProperty, ref _command, value); }
		}

		public OnScrollToEndBehavior()
		{

		}
		protected override void OnAttached()
		{
			base.OnAttached();

			this.AssociatedObject.ScrollChanged += AssociatedObject_ScrollChanged;
		}

		private void AssociatedObject_ScrollChanged(object? sender, ScrollChangedEventArgs e)
		{
			IScrollable scrollable = AssociatedObject as IScrollable;
			if (scrollable.Offset.Y >= AssociatedObject.ScrollBarMaximum.Y)
			{
				if (Command is not null)
				{
					if (Command.CanExecute(null))
						Command.Execute(null);
				}
			}

		}

		protected override void OnDetaching()
		{
			base.OnDetaching();
			this.AssociatedObject.ScrollChanged -= AssociatedObject_ScrollChanged;
		}
	}
}
