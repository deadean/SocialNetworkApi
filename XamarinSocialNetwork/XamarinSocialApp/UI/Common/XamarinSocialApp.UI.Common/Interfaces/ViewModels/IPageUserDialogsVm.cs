using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.UI.Common.Interfaces.ViewModels
{
	public interface IPageUserDialogsVm
	{
		string UserName { get; }
		IUser User { get; }
		ICommand AddNewDialogCommand { get; }
		ICommand RefreshDialogList { get; }
	}
}
