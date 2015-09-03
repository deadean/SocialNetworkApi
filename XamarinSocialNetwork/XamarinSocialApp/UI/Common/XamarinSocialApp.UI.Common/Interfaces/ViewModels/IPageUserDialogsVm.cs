using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.UI.Common.Interfaces.ViewModels
{
	public interface IPageUserDialogsVm
	{
		string UserName { get; }
		IUser User { get; }
	}
}
