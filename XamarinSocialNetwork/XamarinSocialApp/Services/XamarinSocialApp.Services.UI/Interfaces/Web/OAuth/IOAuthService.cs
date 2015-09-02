using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;

namespace XamarinSocialApp.Services.UI.Interfaces.Web.OAuth
{
	public interface IOAuthService
	{
		Task<IUser> Login(enSocialNetwork socialNetwork);
		Task<IEnumerable<IDialog>> GetDialogs(IUser user, enSocialNetwork socialNetwork);
		Task<IEnumerable<IUser>> ShowUserFriends(IUser user, enSocialNetwork enSocialNetwork);
		Task<IUser> GetUserInfoRequest(IUser user, enSocialNetwork enSocialNetwork);
	}
}
