using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;

namespace XamarinSocialApp.Services.UI.Interfaces.Web.OAuth
{
	public interface IOAuthService
	{
		Task<XamarinSocialApp.Data.Interfaces.Entities.OAuth.IUser> Login();
		Task<IEnumerable<IDialog>> GetDialogs(XamarinSocialApp.Data.Interfaces.Entities.Database.IUser user);
	}
}
