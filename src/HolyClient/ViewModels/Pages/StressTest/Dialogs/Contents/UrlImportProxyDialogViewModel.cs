using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.ViewModels;
public sealed class UrlImportProxyDialogViewModel : ImportProxyViewModel
{
	[Reactive]
	public string URL { get; set; }


}
