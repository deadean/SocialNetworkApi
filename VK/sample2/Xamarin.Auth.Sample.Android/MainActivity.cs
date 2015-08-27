using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Xamarin.Auth.Sample.Android
{
	public class User
	{
		public string uid { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
	}


	[Activity (Label = "Xamarin.Auth Sample (Android)", MainLauncher = true)]
	public class MainActivity : Activity
	{
		void LoginToFacebook (bool allowCancel)
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
				request.GetResponseAsync().ContinueWith (t => {
					if (t.IsFaulted) {
					}
					else if (t.IsCanceled) { }
					else
					{
						var response = t.Result.GetResponseText();
						var obj = JsonValue.Parse(response);
						JsonValue resp = obj["response"];

						//JObject jobject = JObject.Parse(response);
						//User u = JsonConvert.DeserializeObject<User>(jobject.ToString());

						JObject jobject1 = JObject.Parse(resp.ToString());
						User u1 = JsonConvert.DeserializeObject<User>(jobject1.ToString());

						//JObject jobject2 = JObject.Parse(jobject["response"].ToString());
						//User u2 = JsonConvert.DeserializeObject<User>(jobject2.ToString());


						string uid = "";
						//string name = resp["first_name"];
						//System.Diagnostics.Debug("Name:", name);
						//System.Diagnostics.Debug("All:", obj);
					}
				}, UIScheduler);
			};

			var intent = auth.GetUI (this);
			StartActivity (intent);
		}

		private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.Main);

			var facebook = FindViewById<Button> (Resource.Id.FacebookButton);			
			facebook.Click += delegate { LoginToFacebook(true);};

			var facebookNoCancel = FindViewById<Button> (Resource.Id.FacebookButtonNoCancel);
			facebookNoCancel.Click += delegate { LoginToFacebook(false);};
		}
	}
}