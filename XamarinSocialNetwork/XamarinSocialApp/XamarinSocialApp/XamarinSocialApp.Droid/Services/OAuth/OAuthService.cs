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

[assembly: Dependency(typeof(OAuthService))]

namespace XamarinSocialApp.Droid.Services.OAuth
{
	public class VkUsers
	{
		public User[] response { get; set; }
	}

	public class VkMessagesResponse
	{
		[JsonProperty("response")]
		public VkMessagesItems Response { get; set; }
	}

	public class VkMessagesItems
	{
		[JsonProperty("count")]
		public int Count { get; set; }

		[JsonProperty("items")]
		public IEnumerable<MessageItem> Messages { get; set; }
	}

	public class User
	{
		public string uid { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
	}

	public class Message
	{
		[JsonProperty("user_id")]
		public string UserId { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }
	}

	public class MessageItem
	{
		[JsonProperty("message")]
		public Message Message { get; set; }
	}

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

		public async Task<bool> Login()
		{
			try
			{
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
						//var builder = new AlertDialog.Builder(this);
						//builder.SetMessage("Not Authenticated");
						//builder.SetPositiveButton("Ok", (o, e) => { });
						//builder.Create().Show();
						return;
					}

					// Now that we're logged in, make a OAuth2 request to get the user's info.
					var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/users.get"), null, ee.Account);
					request.GetResponseAsync().ContinueWith(t =>
					{
						if (t.IsCompleted)
						{
							Token = ee.Account.Properties["access_token"].ToString();
							Account = ee.Account;

							var response = t.Result.GetResponseText();

							var users = JsonConvert.DeserializeObject<VkUsers>(response);

							string uid = users.response[0].uid;
							string firstName = users.response[0].first_name;
							string lastName = users.response[0].last_name;

							//new AlertDialog.Builder(this).SetPositiveButton("Ok", (o, e) => { })
							//														 .SetMessage("You logged in succesfully!")
							//														 .SetTitle("TalkManager")
							//														 .Show();
						}
						else
						{
							//var builder = new AlertDialog.Builder(this);
							//builder.SetMessage("Not Authenticated");
							//builder.SetPositiveButton("Ok", (o, e) => { });
							//builder.Create().Show();
							return;
						}
					}, UIScheduler);
				};

				var intent = auth.GetUI(Forms.Context);

				Forms.Context.StartActivity(intent);
			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

		
	}
}