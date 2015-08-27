using System;
using System.Json;
using System.Threading.Tasks;
using Android.App;
using Android.Widget;
using Android.OS;

namespace Xamarin.Auth.Sample.Android
{
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
					var builder = new AlertDialog.Builder (this);
					if (t.IsFaulted) {
						builder.SetTitle ("Error");
						builder.SetMessage (t.Exception.Flatten().InnerException.ToString());
					} else if (t.IsCanceled)
						builder.SetTitle ("Task Canceled");
					else {
						var obj = JsonValue.Parse (t.Result.GetResponseText());

						builder.SetTitle ("Logged in");
						builder.SetMessage ("Name: " + obj["name"]);
					}

					builder.SetPositiveButton ("Ok", (o, e) => { });
					builder.Create().Show();
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