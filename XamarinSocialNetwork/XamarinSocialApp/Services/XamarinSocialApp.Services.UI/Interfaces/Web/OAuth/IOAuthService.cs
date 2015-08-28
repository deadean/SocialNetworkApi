using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Services.UI.Interfaces.Web.OAuth
{
	public interface IOAuthService
	{
		Task<bool> Login();
	}
}
