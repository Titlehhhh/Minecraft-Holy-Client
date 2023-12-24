using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HolyClient.ViewModels;
public sealed class UrlImportProxyDialogViewModel : ImportProxyViewModel
{
	
	public string URL { get; set; }

	public override bool IsValid()
	{
		return !string.IsNullOrWhiteSpace(URL);
	}
}
