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
				scope: "999999, messages",
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


		public async Task<bool> GetDialogs()
		{
			//Account acc = Account.Deserialize("__username__=&access_token=a2244bcea7d2c16b82beda9b88d63c96cdcddd0ff6d0b4931723cb076c3a6bd91b808455d711c494c3638&expires_in=0&user_id=14926992&state=ntkvxypwhwhrybjj&__cookies__=AAEAAAD%2F%2F%2F%2F%2FAQAAAAAAAAAMAgAAAAZTeXN0ZW0FAQAAABpTeXN0ZW0uTmV0LkNvb2tpZUNvbnRhaW5lcgYAAAANbV9kb21haW5UYWJsZQ9tX21heENvb2tpZVNpemUMbV9tYXhDb29raWVzFW1fbWF4Q29va2llc1BlckRvbWFpbgdtX2NvdW50Dm1fZnFkbk15RG9tYWluAwAAAAABHFN5c3RlbS5Db2xsZWN0aW9ucy5IYXNodGFibGUICAgIAgAAAAkDAAAAABAAACwBAAAUAAAAAQAAAAYEAAAAAAQDAAAAHFN5c3RlbS5Db2xsZWN0aW9ucy5IYXNodGFibGUHAAAACkxvYWRGYWN0b3IHVmVyc2lvbghDb21wYXJlchBIYXNoQ29kZVByb3ZpZGVyCEhhc2hTaXplBEtleXMGVmFsdWVzAAADAwAFBQsIHFN5c3RlbS5Db2xsZWN0aW9ucy5JQ29tcGFyZXIkU3lzdGVtLkNvbGxlY3Rpb25zLklIYXNoQ29kZVByb3ZpZGVyCOxROD8BAAAACgoDAAAACQUAAAAJBgAAABAFAAAAAQAAAAYHAAAABy52ay5jb20QBgAAAAEAAAAJCAAAAAUIAAAAE1N5c3RlbS5OZXQuUGF0aExpc3QBAAAABm1fbGlzdAMdU3lzdGVtLkNvbGxlY3Rpb25zLlNvcnRlZExpc3QCAAAACQkAAAAECQAAACxTeXN0ZW0uQ29sbGVjdGlvbnMuU29ydGVkTGlzdCtTeW5jU29ydGVkTGlzdAkAAAAFX2xpc3QFX3Jvb3QPU29ydGVkTGlzdCtrZXlzEVNvcnRlZExpc3QrdmFsdWVzEFNvcnRlZExpc3QrX3NpemUSU29ydGVkTGlzdCt2ZXJzaW9uE1NvcnRlZExpc3QrY29tcGFyZXISU29ydGVkTGlzdCtrZXlMaXN0FFNvcnRlZExpc3QrdmFsdWVMaXN0AwIFBQAAAwMDHVN5c3RlbS5Db2xsZWN0aW9ucy5Tb3J0ZWRMaXN0CAgcU3lzdGVtLkNvbGxlY3Rpb25zLklDb21wYXJlciVTeXN0ZW0uQ29sbGVjdGlvbnMuU29ydGVkTGlzdCtLZXlMaXN0J1N5c3RlbS5Db2xsZWN0aW9ucy5Tb3J0ZWRMaXN0K1ZhbHVlTGlzdAkKAAAACQsAAAAJDAAAAAkMAAAAAAAAAAAAAAAJDQAAAAoKBAoAAAAdU3lzdGVtLkNvbGxlY3Rpb25zLlNvcnRlZExpc3QHAAAABGtleXMGdmFsdWVzBV9zaXplB3ZlcnNpb24IY29tcGFyZXIHa2V5TGlzdAl2YWx1ZUxpc3QFBQAAAwMDCAgcU3lzdGVtLkNvbGxlY3Rpb25zLklDb21wYXJlciVTeXN0ZW0uQ29sbGVjdGlvbnMuU29ydGVkTGlzdCtLZXlMaXN0J1N5c3RlbS5Db2xsZWN0aW9ucy5Tb3J0ZWRMaXN0K1ZhbHVlTGlzdAkOAAAACQ8AAAABAAAAAQAAAAkQAAAACgkRAAAABAsAAAANU3lzdGVtLk9iamVjdAAAAAAQDAAAAAAAAAAEDQAAABtTeXN0ZW0uQ29sbGVjdGlvbnMuQ29tcGFyZXIBAAAAC0NvbXBhcmVJbmZvAyBTeXN0ZW0uR2xvYmFsaXphdGlvbi5Db21wYXJlSW5mbwkSAAAAEA4AAAAQAAAABhMAAAABLw0PEA8AAAAQAAAACRQAAAANDwUQAAAAJFN5c3RlbS5OZXQuUGF0aExpc3QrUGF0aExpc3RDb21wYXJlcgAAAAACAAAABBEAAAAnU3lzdGVtLkNvbGxlY3Rpb25zLlNvcnRlZExpc3QrVmFsdWVMaXN0AQAAAApzb3J0ZWRMaXN0Ax1TeXN0ZW0uQ29sbGVjdGlvbnMuU29ydGVkTGlzdAkKAAAABBIAAAAgU3lzdGVtLkdsb2JhbGl6YXRpb24uQ29tcGFyZUluZm8DAAAAB2N1bHR1cmUJd2luMzJMQ0lEBm1fbmFtZQAAAQgIGQQAAAAAAAAGFQAAAAVydS1SVQUUAAAAG1N5c3RlbS5OZXQuQ29va2llQ29sbGVjdGlvbgUAAAAJbV92ZXJzaW9uBm1fbGlzdAttX1RpbWVTdGFtcBRtX2hhc19vdGhlcl92ZXJzaW9ucwxtX0lzUmVhZE9ubHkAAwAAAAgcU3lzdGVtLkNvbGxlY3Rpb25zLkFycmF5TGlzdA0BAQIAAAAAAAAACRYAAAAAAAAAAAAAAAEBBBYAAAAcU3lzdGVtLkNvbGxlY3Rpb25zLkFycmF5TGlzdAMAAAAGX2l0ZW1zBV9zaXplCF92ZXJzaW9uBQAACAgJFwAAAAEAAAABAAAAEBcAAAAEAAAACRgAAAANAwUYAAAAEVN5c3RlbS5OZXQuQ29va2llFQAAAAltX2NvbW1lbnQMbV9jb21tZW50VXJpD21fY29va2llVmFyaWFudAltX2Rpc2NhcmQIbV9kb21haW4RbV9kb21haW5faW1wbGljaXQJbV9leHBpcmVzBm1fbmFtZQZtX3BhdGgPbV9wYXRoX2ltcGxpY2l0Bm1fcG9ydA9tX3BvcnRfaW1wbGljaXQLbV9wb3J0X2xpc3QIbV9zZWN1cmUKbV9odHRwT25seQttX3RpbWVTdGFtcAdtX3ZhbHVlCW1fdmVyc2lvbgttX2RvbWFpbktleQ9Jc1F1b3RlZFZlcnNpb24OSXNRdW90ZWREb21haW4BBAQAAQAAAQEAAQAHAAAAAQABAAAKU3lzdGVtLlVyaQIAAAAYU3lzdGVtLk5ldC5Db29raWVWYXJpYW50AgAAAAEBDQEBCAEBDQgBAQIAAAAJBAAAAAoFGQAAABhTeXN0ZW0uTmV0LkNvb2tpZVZhcmlhbnQBAAAAB3ZhbHVlX18ACAIAAAABAAAAAAYaAAAABy52ay5jb20AgBSCGFDM04gGGwAAAAlyZW1peGxhbmcJEwAAAAAJBAAAAAEKAADwKHAxabDSiAYcAAAAATEAAAAACQcAAAAAAAs%3D");
			Account acc = Account;
			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.getDialogs"), null, acc);

			request.Parameters.Add("count", "1");
			request.Parameters.Add("v", "5.37");

			var res = await request.GetResponseAsync();
			var responseText = res.GetResponseText();

			var msg = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkMessagesResponse>(responseText);
			string message = msg.Response.Messages.First().Message.Body;

			return true;
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

	}
}