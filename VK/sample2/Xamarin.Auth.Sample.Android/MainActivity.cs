using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace Xamarin.Auth.Sample.Android
{

	public class VkUsers
	{
		public User[] response { get; set; }
	}

	public class User
	{
		public string uid { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
	}


	[Activity (Label = "Xamarin.Auth Sample (Android)", MainLauncher = true)]
	public class MainActivity : Activity
	{

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

		void LoginToVk (bool allowCancel)
		{
			var auth = new OAuth2Authenticator (
				clientId: "5042701",
				scope: "messages",
				authorizeUrl: new Uri("https://oauth.vk.com/authorize"),
				redirectUrl: new Uri("https://oauth.vk.com/blank.html"));

			auth.AllowCancel = allowCancel;

			// If authorization succeeds or is canceled, .Completed will be fired.
			auth.Completed += (s, ee) => {
				if (!ee.IsAuthenticated) {
					var builder = new AlertDialog.Builder (this);
					builder.SetMessage ("Not Authenticated");
					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
					return;
				}

				// Now that we're logged in, make a OAuth2 request to get the user's info.
				var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/users.get"), null, ee.Account);
				request.GetResponseAsync().ContinueWith (t => 
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

						new AlertDialog.Builder(this).SetPositiveButton("Ok", (o, e) => { })
																				 .SetMessage("You logged in succesfully!")
																				 .SetTitle("TalkManager")
																				 .Show();
					}
					else
					{
						var builder = new AlertDialog.Builder(this);
						builder.SetMessage("Not Authenticated");
						builder.SetPositiveButton("Ok", (o, e) => { });
						builder.Create().Show();
						return;
					}
				}, UIScheduler);
			};


			var intent = auth.GetUI (this);
			StartActivity (intent);
		}


		OAuth2Request GetDialogs()
		{
			//var parameters = new ParamArrayAttribute
			//			{ 
			//				{ "access_token", result.AccessTokenUrl },
			//				{ "count", "10" }
			//};

			//var request = WebRequest.Create("https://api.vk.com/method/groups.isMember.json?count=10&access_token=" + Token);
			//var response = request.GetResponse();


			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.getDialogs?count=10&access_token=" + Token), null, Account);
			request.GetResponseAsync().ContinueWith (t => 
				{
					if (t.IsCompleted)
					{
						new AlertDialog.Builder(this).SetPositiveButton("Ok", (o, e) => { })
																				 .SetMessage("You logged in succesfully!")
																				 .SetTitle("TalkManager")
																				 .Show();
					}
					else
					{
						var builder = new AlertDialog.Builder(this);
						builder.SetMessage("Not Authenticated");
						builder.SetPositiveButton("Ok", (o, e) => { });
						builder.Create().Show();
						return;
					}
				});

			
			return null;
		}


		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var vk = FindViewById<Button> (Resource.Id.VkButton);
			vk.Click += delegate { LoginToVk(true); };

			var facebookNoCancel = FindViewById<Button> (Resource.Id.FacebookButtonNoCancel);
			facebookNoCancel.Click += delegate { GetDialogs();};
		}
	}
}