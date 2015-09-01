using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XamarinSocialApp.UI.Common.Interfaces.ViewModels
{
	public interface IMainPageVm
	{
		ICommand LoginVkCommand { get; }
	}
}
