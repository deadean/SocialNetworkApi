using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.UI.Data.Implementations.Navigation
{
	public class PageDialogWithFriendNavParams
	{
		public PageDialogWithFriendNavParams(IUser user, IUser friend)
		{
			User = user;
			Friend = friend;
		}

		public IUser User { get; private set; }

		public IUser Friend { get; private set; }
	}
}
