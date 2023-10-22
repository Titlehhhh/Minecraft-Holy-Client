using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace HolyClient.CustomControls
{
	[TemplatePart("PART_StackPanel", typeof(StackPanel))]
	public class FillStackPanel : TemplatedControl
	{
		public static StyledProperty<IDataTemplate> ItemTemplateProperty =
			AvaloniaProperty.Register<FillStackPanel, IDataTemplate>(nameof(ItemTemplate));

		public IDataTemplate ItemTemplate
		{
			get => this.GetValue<IDataTemplate>(ItemTemplateProperty);
			set
			{
				this.SetValue<IDataTemplate>(ItemTemplateProperty, value);
			}
		}
		public FillStackPanel()
		{

		}
		private StackPanel _panel;
		protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
		{
			_panel = e.NameScope.Find<StackPanel>("PART_StackPanel");
			base.OnApplyTemplate(e);
		}
		private double ItemHeight = 48 + 10;
		protected override void OnSizeChanged(SizeChangedEventArgs e)
		{
			int count = (int)(e.NewSize.Height / ItemHeight) + 1;

			if (_panel.Children.Count > count)
			{

			}
			else if (_panel.Children.Count < count)
			{
				int delta = count - _panel.Children.Count;
				for (int i = 0; i < delta; i++)
				{

				}
			}

			base.OnSizeChanged(e);
		}


	}

}
