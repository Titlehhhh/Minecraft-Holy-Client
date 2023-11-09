using Avalonia.ReactiveUI;
using HolyClient.ViewModels;
using ReactiveUI;

namespace HolyClient.Views.Pages.StressTest;

public partial class StressTestProcessView : ReactiveUserControl<IStressTestProcessViewModel>
{
	public StressTestProcessView()
	{
		InitializeComponent();

		this.WhenActivated((d) => {

			this.ViewModel.Logs.CollectionChanged += Logs_CollectionChanged;
		
		});

		//ProxyPieChart.LegendPosition = LiveChartsCore.Measure.LegendPosition.Top;
		//ProxyPieChart.LegendTextPaint = new SolidColorPaint(SKColors.White);
		//ProxyPieChart.LegendTextSize = 16;

		//ProxyPieChart.DrawMargin = new Margin(50, 50, 50, 0);
		//ProxyPieChart.Width = 250;
		//ProxyPieChart.Title = new LabelVisual
		//{
		//	Text = "Прокси",
		//	TextSize = 16,
		//	Paint = new SolidColorPaint(SKColors.White)
		//};
	}

	private void Logs_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
	{
		LogsScroll.ScrollToEnd();
	}
}