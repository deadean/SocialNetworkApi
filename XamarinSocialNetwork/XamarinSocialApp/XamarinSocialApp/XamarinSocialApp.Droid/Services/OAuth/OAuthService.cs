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
using Newtonsoft.Json.Linq;
using Xamarin.Auth;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;
using XamarinSocialApp.UI.Data.Implementations.Entities.OAuth;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Common.Enums;
using DataDialogs = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.Dialog;
using DataIUser = XamarinSocialApp.Data.Interfaces.Entities.Database.IUser;
using DataUser = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.User;
using DataMessage = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.Message;
using MessagesUI = XamarinSocialApp.UI.Data.Implementations.Messages.Messages;

using LinqToTwitter;
using Account = Xamarin.Auth.Account;
using User = XamarinSocialApp.UI.Data.Implementations.Entities.Databases.User;
using XamarinSocialApp.Droid.Data;

[assembly: Dependency(typeof(OAuthService))]

namespace XamarinSocialApp.Droid.Services.OAuth
{
	public class OAuthService : IOAuthService
	{

		#region Fields

		private int currentCountQueue;

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
			currentCountQueue = 0;
		}

		#endregion

		#region Public Methods

		public async Task RegisterInLongPoolServer(IUser user)
		{
			try
			{
				this.StartRequest();

				Account acc = Account.Deserialize(user.SerializeInfo.ToString());

				var request = 
					new OAuth2Request("GET", 
						new Uri("https://api.vk.com/method/messages.getLongPollServer?access_token="+Token), 
				null, acc);

				request.Parameters.Add("use_ssl", "1");
				request.Parameters.Add("need_pts", "0");

				var res1 = await request.GetResponseAsync();
				var responseText = res1.GetResponseText();

				var settings = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkLongPoolServerResponse>(responseText);

				this.StopRequest();

				this.StartRequest();

				while (true)
				{
					request = new OAuth2Request("GET",
							new Uri(String.Format("http://{0}?act=a_check&key={1}&ts={2}&wait=25&mode=2",
							settings.Response.ServerUrl, settings.Response.Key, settings.Response.Ts
					)), null, acc);
					res1 = await request.GetResponseAsync();
					responseText = res1.GetResponseText();
					var updates =
						JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkLongPoolServerUpdates>
						(responseText);
					settings.Response.Ts = updates.Ts;

					foreach (var updateArray in updates.Updates)
					{
						JToken token0 = updateArray[0];
						int operation = (int)token0;
						if (operation != 4)
							continue;

						int uidSender = (int)updateArray[3];
						int messageId = (int)updateArray[1];
						int dialogParticipientId = (int)updateArray[2];
						string messageString = (string)updateArray[6];

						await SendRecievedMessage(user, acc, uidSender, messageId, dialogParticipientId, messageString);
					}
				}

				this.StopRequest();

			}
			catch (Exception ex)
			{
				RegisterInLongPoolServer(user);
			}
		}

		private async Task SendRecievedMessage(IUser user, Account acc, int uidSender, int messageId, int dialogParticipientId, string messageString)
		{
			string messageType = await GetMessageSenderMessageType(messageId, acc);
			var sender = new DataUser() { Uid = uidSender.ToString() };

			IMessage message = new DataMessage()
			{
				Content = messageString,
				Recipient = messageType == "1" ? sender : user,
				Sender = messageType == "1" ? user : sender,
				ParticipientId = dialogParticipientId.ToString()
			};

			if (messageType == "1")
			{
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<MessagesUI.MessageNewMyMessageWasSent>
				(new MessagesUI.MessageNewMyMessageWasSent(message));
			}
			else
			{
				GalaSoft.MvvmLight.Messaging.Messenger.Default.Send<MessagesUI.MessageNewMessageWasSentToMe>
				(new MessagesUI.MessageNewMessageWasSentToMe(message));
			}
		}

		private async Task<string> GetMessageSenderMessageType(int messageId, Account acc)
		{
			try
			{
				this.StartRequest();

				var request =
					new OAuth2Request("GET",
						new Uri("https://api.vk.com/method/messages.getById"),
				null, acc);

				request.Parameters.Add("message_ids", messageId.ToString());
				request.Parameters.Add("preview_length", "0");
				request.Parameters.Add("v", "5.37");

				var res1 = await request.GetResponseAsync();
				var responseText = res1.GetResponseText();

				var response = 
					JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkMessageByIdResponse>(responseText);

				return response.Response.Messages.First().Out;
			}
			catch (Exception ex)
			{

			}
			finally
			{
				this.StopRequest();
			}

			return String.Empty;
		}

		public async Task<bool> SendMessage(DataIUser user, DataIUser friend, string Message, enSocialNetwork enSocialNetwork)
		{
			try
			{
				this.StartRequest();
				Account acc = Account.Deserialize(user.SerializeInfo.ToString());
				var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.send"), null, acc);

				request.Parameters.Add("user_id", friend.Uid);
				request.Parameters.Add("message", Message);

				var res1 = await request.GetResponseAsync();
				var responseText = res1.GetResponseText();

			}
			catch (Exception ex)
			{
				return false;
			}
			finally
			{
				this.StopRequest();
			}

			return true;
		}

		public async Task<IUser> Login(enSocialNetwork socialNetwork)
		{
			IUser user = null;
			try
			{
				switch(socialNetwork)
				{
					case enSocialNetwork.VK:
						user = await LoginToVk();
						break;
					case enSocialNetwork.Twitter:
						user = await LoginToTwitter();
						break;

					default:
						break;
				}
			}
			catch (Exception ex)
			{
			}

			return user;
		}

		private static async Task<DataIUser> LoginToVk()
		{
			IUser user = null;
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

							user = new User()
							{
								FirstName = firstName,
								LastName = lastName,
								Uid = uid,
								SerializeInfo = Account.Serialize(),
								SocialNetwork = enSocialNetwork.VK
							};

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
			catch (Exception)
			{
			}
			return user;
		}

		private async Task<DataIUser> LoginToTwitter()
		{
			DataIUser resultUser = null;
			LinqToTwitter.User twitterUser = null;
			try
			{
				TaskCompletionSource<int> ts = new TaskCompletionSource<int>();

				var auth = new OAuth1Authenticator(
										consumerKey: "YVgafJLg6figpxcFTx9oBhXDw",
										consumerSecret: "AdNdiuHSHIf5hN6HWnVrC9u6bnW3vVirjEhAFrfabacPIQdh98",
					requestTokenUrl: new Uri("https://api.twitter.com/oauth/request_token"),
					//https://api.twitter.com/oauth/authorize
					authorizeUrl: new Uri("https://api.twitter.com/oauth/authorize"),
					accessTokenUrl: new Uri("https://api.twitter.com/oauth/access_token"),
					callbackUrl: new Uri("https://oauth.vk.com/blank.html"));

				auth.AllowCancel = true;

				auth.Completed += (s, ee) =>
				{
					if (!ee.IsAuthenticated)
					{
						ts.SetResult(0);
						return;
					}

					var twitterCtx = GetTwitterContext();

					var accounts =
						from acct in twitterCtx.Account
						where acct.Type == AccountType.VerifyCredentials
						select acct;

					LinqToTwitter.Account account = accounts.SingleOrDefault();

					twitterUser = account.User;

					#region unused requests

					//var searchResponses = (from search in ctx.Search
					//											 where search.Type == SearchType.Search
					//											 && search.Query == searchText
					//											 select search).SingleOrDefault();

					//var tweets = from tweet in searchResponses.Statuses
					//						 select new Message
					//						 {
					//							 Value = tweet.Text,
					//							 Id = tweet.TweetIDs,
					//							 ImageUri = tweet.User.ProfileImageUrl,
					//							 UserName = tweet.User.ScreenNameResponse,
					//							 Name = tweet.User.Name,
					//							 CreatedAt = tweet.CreatedAt,
					//							 ReTweets = tweet.RetweetCount,
					//							 Favorite = tweet.FavoriteCount.Value
					//						 };

					//var followers = (from follower in twitterCtx.Friendship
					//								 where follower.Type == FriendshipType.FollowersList &&
					//											 follower.UserID == "3620214675"
					//								 select follower).SingleOrDefault();

					#endregion

					string uid = twitterUser.UserIDResponse;
					string firstName = twitterUser.ScreenNameResponse;

					TwitterData.TwitterUser twUser = new TwitterData.TwitterUser();
					twUser.ConsumerKey = twitterCtx.Authorizer.CredentialStore.ConsumerKey;
					twUser.ConsumerSecret = twitterCtx.Authorizer.CredentialStore.ConsumerSecret;
					twUser.OAuthToken = twitterCtx.Authorizer.CredentialStore.OAuthToken;
					twUser.OAuthTokenSecret = twitterCtx.Authorizer.CredentialStore.OAuthTokenSecret;

					twUser.UserID = ulong.Parse(uid);
					twUser.ScreenName = firstName;

					resultUser = new User()
					{
						FirstName = firstName,
						Uid = uid,
						SerializeInfo = JsonConvert.SerializeObject(twUser),
						SocialNetwork = enSocialNetwork.Twitter				
					};

					ts.SetResult(0);

					//var followers = (from follower in twitterCtx.Friendship
					//								 where follower.Type == FriendshipType.FollowersList &&
					//											 follower.UserID == user.UserIDResponse
					//								 select follower.Users).ToList();

					//string friendName = followers.SingleOrDefault().FirstOrDefault().Name;

					//var msgs =
					//(from dm in twitterCtx.DirectMessage
					// where dm.Type == DirectMessageType.SentTo
					// select dm).ToList();

					//string message = msgs.FirstOrDefault().Text;

				};

				var intent = auth.GetUI(Forms.Context);
				Forms.Context.StartActivity(intent);
				await ts.Task;
			}
			catch (Exception)
			{
			}
			return resultUser;
		}
		
		public async Task<IEnumerable<IDialog>> GetDialogs(DataIUser user, enSocialNetwork socialNetwork)
		{
			IEnumerable<IDialog> dialogs = null;
			switch(socialNetwork)
			{
				case enSocialNetwork.VK:
					dialogs = await GetVkDialogs(user);
					break;

				case enSocialNetwork.Twitter:
					dialogs = await GetTwitterDialogs(user);
					break;
										
			}
			return dialogs;
		}

		public async Task<IEnumerable<IDialog>> GetTwitterDialogs(DataIUser user)
		{
			IList<IDialog> dialogs = new List<IDialog>();
			try
			{
				TwitterContext context = this.GetTwitterContext(user.SerializeInfo);

				var twitterDialogs = await
						(from dm in context.DirectMessage
						 where dm.Type == DirectMessageType.SentTo
						 select dm)
						.ToListAsync();

				foreach (var msg in twitterDialogs)
				{
					IUser userDialog = new User()
					{
						Uid = msg.RecipientID.ToString(),
						SerializeInfo = user.SerializeInfo
					};

					dialogs.Add(new DataDialogs(userDialog, new List<IMessage>() 
					{ 
						new DataMessage() { Content = msg.Text } 
					}));
				}
			}
			catch (Exception)
			{
			}

			return dialogs;
		}

		private static async Task<IEnumerable<IDialog>> GetVkDialogs(DataIUser user)
		{
			IList<IDialog> dialogs = new List<IDialog>();
			try
			{
				Account acc = Account.Deserialize(user.SerializeInfo.ToString());
				var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.getDialogs"), null, acc);

				request.Parameters.Add("count", "200");
				request.Parameters.Add("v", "5.37");

				var res = await request.GetResponseAsync();
				var responseText = res.GetResponseText();

				var msg = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkDialogsResponse>(responseText);


				foreach (var item in msg.Response.Messages)
				{
					IUser userDialog = new User()
					{
						Uid = item.Message.UserId,
						SerializeInfo = user.SerializeInfo
					};
					//var userDialog = await GetUserInfoRequest(item.Message.UserId, acc, socialNetwork);
					dialogs.Add(new DataDialogs(userDialog, new List<IMessage>() 
				{ 
					new DataMessage() { Content = item.Message.Body } 
				}));
				}
			}
			catch (Exception)
			{
			}

			return dialogs;
		}

		public async Task<IEnumerable<DataIUser>> GetUserFriends(DataIUser user, enSocialNetwork enSocialNetwork)
		{
			Account acc = Account.Deserialize(user.SerializeInfo.ToString());
			var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/friends.get"), null, acc);

			request.Parameters.Add("fields", "nickname,photo_200");
			request.Parameters.Add("order", "hints");

			var res = await request.GetResponseAsync();
			var responseText = res.GetResponseText();

			var listFriendsIds = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(responseText);

			IList<DataIUser> friends = new List<DataIUser>();

			foreach (var friend in listFriendsIds.response)
			{
				friends.Add(new DataUser() 
				{
					UserPhoto = friend.photo_200,
					FirstName = friend.first_name, 
					LastName = friend.last_name, 
					SerializeInfo = user.SerializeInfo,
					Uid = friend.uid
				});
			}

			return friends;
		}

		public async Task<IUser> GetUserInfoRequest(IUser user, enSocialNetwork socialNetwork)
		{
			if (user.HasNotValue())
				return null;

			switch (socialNetwork)
			{
				case enSocialNetwork.VK:

					break;
				case enSocialNetwork.Twitter:

					break;

				default:
					break;
			}

			Account accCurrent = Account.Deserialize(user.SerializeInfo.ToString());
			if (accCurrent.HasNotValue())
				return null;

			if (String.IsNullOrWhiteSpace(user.Uid))
				return null;

			this.StartRequest();

			try
			{
				var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/users.get"), null, accCurrent);
				request.Parameters.Add("uids", user.Uid);

				var res = await request.GetResponseAsync();
				var responseText = res.GetResponseText();

				var users = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkUsers>(responseText);

				var jsonUser = users.response.First();
				return new DataUser() 
				{
					FirstName = jsonUser.first_name,
					LastName = jsonUser.last_name,
					ID = jsonUser.uid,
					Uid = jsonUser.uid
				};
			}
			catch (Exception ex)
			{

			}
			finally
			{
				this.StopRequest();
			}

			return null;
		}

		public async Task<IDialog> GetDialogWithFriend(DataIUser user, enSocialNetwork socialNetwork, IUser friend)
		{
			IDialog dialog = null;

			try
			{
				Account acc = Account.Deserialize(user.SerializeInfo.ToString());

				this.StartRequest();

				var request = new OAuth2Request("GET", new Uri("https://api.vk.com/method/messages.getHistory"), null, acc);

				request.Parameters.Add("user_id", friend.Uid);
				request.Parameters.Add("count", "200");

				var res = await request.GetResponseAsync();
				var responseText = res.GetResponseText();

				var msg = JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.VkMessagesResponse>(responseText);
				msg.Response.RemoveAt(0);

				IList<XamarinSocialApp.Droid.Data.VkData.MessageInDialog> msg1
					= new List<XamarinSocialApp.Droid.Data.VkData.MessageInDialog>();
				foreach (var item in msg.Response)
				{
					msg1.Add(JsonConvert.DeserializeObject<XamarinSocialApp.Droid.Data.VkData.MessageInDialog>(item.ToString()));
				}

				IList<IMessage> messages = new List<IMessage>();

				foreach (var item in msg1)
				{
					messages.Add(new DataMessage() { Content = item.Body, Sender = item.UserId == user.Uid ? user : friend });
				}


				dialog = new XamarinSocialApp.UI.Data.Implementations.Entities.Databases.Dialog(user, messages);
			}
			catch (Exception ex)
			{

			}

			this.StopRequest();

			return dialog;
		}

		#endregion

		#region Private Methods

		private void StartRequest()
		{
			while (true)
			{
				if(currentCountQueue<2)
				{
					currentCountQueue++;
					return;
				}
			}
		}

		private void StopRequest()
		{
			currentCountQueue--;
		}

		private TwitterContext GetTwitterContext(string serializeInfo = "")
		{
			TwitterData.TwitterUser user = JsonConvert.DeserializeObject<TwitterData.TwitterUser>(serializeInfo);
			var auth = new XAuthAuthorizer()
			{
				//CredentialStore = new InMemoryCredentialStore
				//{
				//	ConsumerKey = "YVgafJLg6figpxcFTx9oBhXDw",
				//	ConsumerSecret = "AdNdiuHSHIf5hN6HWnVrC9u6bnW3vVirjEhAFrfabacPIQdh98",
				//	OAuthToken = "3620214675-KzcSqmYy131LlcQGe8nOptxCdQCBP8ajPYXYwvl",
				//	OAuthTokenSecret = "GycvXtklGfaYz1WjOINEhkmZr6OwGDz38SUX68iCMWv9f",
				//	ScreenName = "bogdanm__",
				//	UserID = 3620214675
				//},
				CredentialStore = new InMemoryCredentialStore
				{
					ConsumerKey = "YVgafJLg6figpxcFTx9oBhXDw",
					ConsumerSecret = "AdNdiuHSHIf5hN6HWnVrC9u6bnW3vVirjEhAFrfabacPIQdh98",
					OAuthToken = "3620214675-KzcSqmYy131LlcQGe8nOptxCdQCBP8ajPYXYwvl",
					OAuthTokenSecret = "GycvXtklGfaYz1WjOINEhkmZr6OwGDz38SUX68iCMWv9f",
					ScreenName = user == null ? String.Empty : user.ScreenName,
					UserID = user == null ? 0 : user.UserID
				},
			};
			auth.AuthorizeAsync();

			var ctx = new TwitterContext(auth);
			return ctx;
		}

		#endregion

		#region Protected Methods

		#endregion
	}
}