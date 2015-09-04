using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;
using Xamarin.Forms;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using XamarinSocialApp.WinRT.Tablet.Implementations.Services.OAuth;

[assembly: Dependency(typeof(OAuthService))]

namespace XamarinSocialApp.WinRT.Tablet.Implementations.Services.OAuth
{
	public class OAuthService : IOAuthService
	{

		#region Services

		#endregion

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Ctor

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		public async Task<IUser> Login(enSocialNetwork socialNetwork)
		{
			IUser user = null;
			try
			{
				String vkUrl =
					String.Format("https://oauth.vk.com/authorize?client_id={0}&scope={1}&response_type=token"
					, 5042701, "offline,messages,friends");
				String vkCallBackUrl = String.Format("https://oauth.vk.com/blank.html");

				System.Uri StartUri = new Uri(vkUrl);
				System.Uri EndUri = new Uri(vkCallBackUrl);

				WebAuthenticationResult response = 
					await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,StartUri,EndUri);

				if (response.ResponseStatus == WebAuthenticationStatus.Success)
				{
					user = await GetVkUser(response.ResponseData.ToString());
				}

			}
			catch (Exception ex)
			{

			}

			return user;
		}

		public async Task<IEnumerable<IDialog>> GetDialogs(IUser user, enSocialNetwork socialNetwork)
		{
			IList<IDialog> dialogs = null;
			try
			{
				String url =
					String.Format("https://api.vk.com/method/messages.getDialogs?&count={0}&access_token={1}"
					, 100.ToString(), user.SerializeInfo);

				HttpClient httpClient = new HttpClient();
				string response =
					await httpClient.GetStringAsync(new Uri(url));

				string[] responseString = response.Split('[');
				string[] responseDialogsArray = responseString[1].Split('{');

				dialogs = new List<IDialog>();
				foreach (var item in responseDialogsArray)
				{
					try
					{
						string uid = item.Split(',')[3].Split(':')[1];
						string message = String.Empty;
						try
						{
							message = item.Split(',')[6].Split(':')[1];
						}
						catch (Exception ex)
						{
							message = item.Split(',')[6].Split('\"')[0].Trim();
						}

						IUser friend = new User() { Uid = uid, SerializeInfo = user.SerializeInfo };
						IMessage friendMessage = new Message() { Content = message };
						IDialog dialog = new Dialog(friend, new List<IMessage>() { friendMessage });
						dialogs.Add(dialog);
					}
					catch (Exception ex)
					{

					}
				}
			}
			catch (Exception ex)
			{

			}

			return dialogs;
		}

		public Task<IEnumerable<IUser>> GetUserFriends(IUser user, enSocialNetwork enSocialNetwork)
		{
			throw new NotImplementedException();
		}

		public Task<IDialog> GetDialogWithFriend(IUser user, enSocialNetwork socialNetwork, IUser friend)
		{
			throw new NotImplementedException();
		}

		public async Task<IUser> GetUserInfoRequest(IUser user, enSocialNetwork enSocialNetwork)
		{
			try
			{
				HttpClient httpClient = new HttpClient();
				string urlGetUser =
					String.Format("https://api.vk.com/method/users.get?token={0}&user_ids={1}",
					user.SerializeInfo, user.Uid
					);

				string response =
					await httpClient.GetStringAsync(new Uri(urlGetUser));

				string firstName = response.Split('[')[1].Split(',')[1].Split('\"')[3];
				string lastName = response.Split('[')[1].Split(',')[2].Split('\"')[3];

				return new User() { FirstName = firstName, LastName = lastName, Uid = user.Uid, SerializeInfo = user.SerializeInfo };
			}
			catch (Exception ex)
			{

			}
			return null;
		}

		public Task<bool> SendMessage(IUser user, IUser friend, string Message, enSocialNetwork enSocialNetwork)
		{
			throw new NotImplementedException();
		}

		public Task RegisterInLongPoolServer(IUser user)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Private Methods

		private async Task<IUser> GetVkUser(string webAuthResultResponseData)
		{
			string responseData = webAuthResultResponseData.Substring(webAuthResultResponseData.IndexOf("access_token"));
			String[] keyValPairs = responseData.Split('&');
			string access_token = null;
			string expires_in = null;
			string uid = null;
			for (int i = 0; i < keyValPairs.Length; i++)
			{
				String[] splits = keyValPairs[i].Split('=');
				switch (splits[0])
				{
					case "access_token":
						access_token = splits[1];
						break;
					case "expires_in":
						expires_in = splits[1];
						break;
					case "user_id":
						uid = splits[1];
						break;
				}
			}

			HttpClient httpClient = new HttpClient();
			string urlGetUser = 
				String.Format("https://api.vk.com/method/users.get?token={0}&user_ids={1}",
				access_token, uid
				);
			string response =
				await httpClient.GetStringAsync(new Uri(urlGetUser));

			string firstName = response.Split('[')[1].Split(',')[1].Split('\"')[3];
			string lastName = response.Split('[')[1].Split(',')[2].Split('\"')[3];

			return new User() { FirstName = firstName, LastName = lastName, Uid = uid, SerializeInfo = access_token };
		}

		#endregion

		#region Protected Methods

		#endregion
		
	}
}
