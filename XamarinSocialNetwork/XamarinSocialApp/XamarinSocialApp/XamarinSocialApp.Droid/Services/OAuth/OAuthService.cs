using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.Droid.Services.OAuth;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Auth;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;
using XamarinSocialApp.UI.Data.Implementations.Entities.OAuth;

[assembly: Dependency(typeof(OAuthService))]

namespace XamarinSocialApp.Droid.Services.OAuth
{
	

	public class OAuthService : IOAuthService
	{

		#region Fields

		#endregion

		#region Properties

		static string mvToken;
		public static string Token
		{
			get { return mvToken; }
			set { mvToken = value; }
		}

		static Account mvAccount;
		public static Account Account
		{
			get { return mvAccount; }
			set { mvAccount = value; }
		}

		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		#endregion

		#region Ctor

		public OAuthService()
		{
			
		}

		#endregion

		#region Public Methods

		public async Task<IUser> Login()
		{
			IUser user = null;
			try
			{
				TaskCompletionSource<int> ts = new TaskCompletionSource<int>();

				var auth = new OAuth2Authenticator(
				clientId: "5042701",
				scope: "messages",
				authorizeUrl: new Uri("https://oauth.vk.com/authorize"),
				redirectUrl: new Uri("https://oauth.vk.com/blank.html"));

				auth.AllowCancel = true;

				auth.Completed += (s, ee) =>
				{
					if (!ee.IsAuthenticated)
					{
						ts.SetResult(0);
						return;
					}

					var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/users.get"), null, ee.Account);
					request.GetResponseAsync().ContinueWith(t =>
					{
						if (t.IsCompleted)
						{
							Token = ee.Account.Properties["access_token"].ToString();
							Account = ee.Account;

							var response = t.Result.GetResponseText();
							var users = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(response);

							string uid = users.response[0].uid;
							string firstName = users.response[0].first_name;
							string lastName = users.response[0].last_name;

							user = new User(uid, firstName, lastName);

							ts.SetResult(0);
						}
						else
						{
							ts.SetResult(0);
							return;
						}
					}, UIScheduler);
				};

				var intent = auth.GetUI(Forms.Context);
				Forms.Context.StartActivity(intent);
				await ts.Task;
			}
			catch (Exception ex)
			{
			}

			return user;
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

		
	}
}