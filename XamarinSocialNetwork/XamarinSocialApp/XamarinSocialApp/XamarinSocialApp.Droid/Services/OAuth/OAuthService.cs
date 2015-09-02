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
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Common.Enums;
using DataDialogs = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.Dialog;
using DataIOAuthUser = XamarinSocialApp.Data.Interfaces.Entities.OAuth.IUser;
using DataIUser = XamarinSocialApp.Data.Interfaces.Entities.Database.IUser;
using DataUser = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.User;
using DataMessage = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.Message;

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

		public async Task<DataIOAuthUser> Login(enSocialNetwork socialNetwork)
		{
			XamarinSocialApp.Data.Interfaces.Entities.OAuth.IUser user = null;
			try
			{
				TaskCompletionSource<int> ts = new TaskCompletionSource<int>();

				var auth = new OAuth2Authenticator(
				clientId: "5042701",
				scope: "offline,messages,friends",
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
							string account = ee.Account.Serialize();
							Account = ee.Account;

							//AccountStore.Create(Forms.Context).Save(ee.Account, "Vk");
							
							var response = t.Result.GetResponseText();
							var users = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(response);

							string uid = users.response[0].uid;
							string firstName = users.response[0].first_name;
							string lastName = users.response[0].last_name;

							user = new User(uid, firstName, lastName, Account.Serialize(), enSocialNetwork.VK);

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
		
		public async Task<IEnumerable<IDialog>> GetDialogs(DataIUser user, enSocialNetwork socialNetwork)
		{
			Account acc = Account.Deserialize(user.SerializeInfo);
			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.getDialogs"), null, acc);

			//request.Parameters.Add("count", "");
			request.Parameters.Add("v", "5.37");

			var res = await request.GetResponseAsync();
			var responseText = res.GetResponseText();

			var msg = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkMessagesResponse>(responseText);

			IList<IDialog> dialogs = new List<IDialog>();

			foreach (var item in msg.Response.Messages)
			{
				var userDialog = await GetUserInfoRequest(item.Message.UserId, acc);
				dialogs.Add(new DataDialogs(userDialog, new List<IMessage>() 
				{ 
					new DataMessage() { Content = item.Message.Body } 
				}));

				await Task.Delay(400);
			}

			return dialogs;
		}


		public async Task<IEnumerable<DataIUser>> ShowUserFriends(DataIUser user, enSocialNetwork enSocialNetwork)
		{
			Account acc = Account.Deserialize(user.SerializeInfo);
			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/friends.get"), null, acc);

			request.Parameters.Add("fields", "nickname");
			request.Parameters.Add("order", "hints");

			var res = await request.GetResponseAsync();
			var responseText = res.GetResponseText();

			var listFriendsIds = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(responseText);

			IList<DataIUser> friends = new List<DataIUser>();

			foreach (var friend in listFriendsIds.response)
			{
				friends.Add(new DataUser() { FirstName = friend.first_name, LastName = friend.last_name });
			}

			return friends;
		}



		#endregion

		#region Private Methods

		private async Task<DataIUser> GetUserInfoRequest(string uid, Account acc)
		{
			if (String.IsNullOrWhiteSpace(uid))
				return null;

			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/users.get"), null, acc);
			request.Parameters.Add("uids", uid);

			var res = await request.GetResponseAsync();
			var responseText = res.GetResponseText();

			var users = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(responseText);

			var jsonUser = users.response.First();
			return new DataUser() { FirstName = jsonUser.first_name, LastName = jsonUser.last_name, ID = jsonUser.uid };
		}

		#endregion

		#region Protected Methods

		#endregion

	}
}