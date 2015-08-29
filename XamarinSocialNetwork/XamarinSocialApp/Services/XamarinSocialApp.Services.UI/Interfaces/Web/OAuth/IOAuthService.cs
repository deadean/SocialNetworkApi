using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;

namespace XamarinSocialApp.Services.UI.Interfaces.Web.OAuth
{
	public interface IOAuthService
	{
		Task<IUser> Login();
		Task<bool> GetDialogs();
	}
}
