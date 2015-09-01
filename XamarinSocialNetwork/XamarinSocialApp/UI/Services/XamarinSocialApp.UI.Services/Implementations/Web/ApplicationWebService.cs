using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;

namespace XamarinSocialApp.UI.Services.Implementations.Web
{
	public class ApplicationWebService : IApplicationWebService
	{
		#region Services

		#endregion

		#region Fields

		private readonly IOAuthService modService;

		#endregion

		#region Properties


		#endregion

		#region Ctor

		public ApplicationWebService(IOAuthService service)
		{
			modService = service;
		}

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		public async Task<IUser> Login(enSocialNetwork socialNetwork)
		{
			XamarinSocialApp.Data.Interfaces.Entities.OAuth.IUser user = await modService.Login(socialNetwork);
			return new User() 
			{ 
				FirstName = user.FirstName, 
				LastName = user.LastName, 
				SerializeInfo = user.SerializeInfo, 
				SocialNetwork = user.SocialNetwork 
			};
		}

		public Task<IEnumerable<IDialog>> GetDialogs(IUser user)
		{
			return modService.GetDialogs(user, user.SocialNetwork);
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion




		
	}
}
