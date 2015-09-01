using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.Services.UI.Interfaces.Web
{
	public interface IApplicationWebService
	{
		Task<IUser> Login(enSocialNetwork socialNetwork);

		Task<IEnumerable<IDialog>> GetDialogs(IUser user);
	}
}
