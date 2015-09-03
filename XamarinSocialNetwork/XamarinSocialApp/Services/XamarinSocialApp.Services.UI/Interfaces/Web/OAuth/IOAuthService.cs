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
		Task<IEnumerable<IUser>> GetUserFriends(IUser user, enSocialNetwork enSocialNetwork);
		Task<IDialog> GetDialogWithFriend(IUser user, enSocialNetwork socialNetwork, IUser friend);
		Task<IUser> GetUserInfoRequest(IUser user, enSocialNetwork enSocialNetwork);
		Task<bool> SendMessage(IUser user, IUser friend, string Message, enSocialNetwork enSocialNetwork);
	}
}
