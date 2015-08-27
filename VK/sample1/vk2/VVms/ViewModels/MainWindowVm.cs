using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using vk2.Commands;

namespace vk2.VVms.ViewModels
{
	public class MainWindowVm : ViewModelBase
	{
		enum enVkApi
		{
			getAllMessages,
			getAllDialogs,
			sendMessage
		}

		#region Fields

		private Dictionary<enVkApi, string> modApi = new Dictionary<enVkApi, string>();

		#endregion

		#region Properties

		private ObservableCollection<string> mvMessages = new ObservableCollection<string>();
		public ObservableCollection<string> Messages
		{
			get { return mvMessages; }
			set
			{
				if (ReferenceEquals(mvMessages, value))
					return;

				mvMessages = value;
				this.RaisePropertyChanged("Messages");
			}
		}

		#endregion

		#region Commands

		private ICommand mvStartCommand;
		public ICommand StartCommand
		{
			get
			{
				return mvStartCommand;
			}

			set
			{
				mvStartCommand = value;
				this.RaisePropertyChanged("StartCommand");
			}
		}
		

		public RelayCommand LoadMsgInDialogCommand { get; private set; }

		#endregion

		#region Command Execute Handlers

		private void OnStartCommand()
		{
			WebBrowser.Navigate(OAuthWeb.GetAuthorizationUrl("VK"));
		}

		#endregion

		#region Private Methods

		#endregion


		#region Ctor

		public MainWindowVm()
		{
			modApi.Add(enVkApi.getAllMessages, "https://api.vk.com/method/messages.get");
			modApi.Add(enVkApi.getAllDialogs, "https://api.vk.com/method/messages.getDialogs");
			modApi.Add(enVkApi.sendMessage, "https://api.vk.com/method/messages.send");

			RegisterClient();

			StartCommand = new DelegateCommand(OnStartCommand);
		}

		#endregion

		#region Register API-Clients

		private void RegisterClient()
		{
			OAuthManager.RegisterClient
			(
				new VkontakteClient
				(
					"5042701",
					"JzLKIvmVK2hrLqd7NeOH"
				)
				{
					Parameters = new NameValueCollection { { "display", "popup" } },
					Scope = "friends, messages"
				}
			);
		}

		#endregion


		#region Load messages in dialog

		#endregion

		#region Requests to API

		private static string GetUserInfoRequest(UniValue param)
		{
			if (param == null)
				return null;

			var parameters = new NameValueCollection 
					{ 
						{ "uids", param["uid"].ToString()	}
					};

			var getUserInfoRequest = Nemiro.OAuth.OAuthUtility.ExecuteRequest
			(
				"GET",
				"https://api.vk.com/method/users.get",
				parameters,
				null
			);

			string userName = String.Empty;

			foreach (UniValue item in getUserInfoRequest["response"])
			{
				userName = String.Format("{0} {1}", item["first_name"], item["last_name"]);
			}

			return userName;
		}

		private static RequestResult GetDialogsRequest(AuthorizationResult result)
		{
			if (result == null)
				return null;

			var parameters = new NameValueCollection
						{ 
							{ "access_token", result.AccessTokenValue },
							{ "count", "10" }
			};

			var getDialogsRequest = Nemiro.OAuth.OAuthUtility.ExecuteRequest
			(
				"GET",
				"https://api.vk.com/method/messages.getDialogs",
				parameters,
				null
			);

			return getDialogsRequest;
		}

		private static RequestResult SendMessageRequest(AuthorizationResult result)
		{
			if (result == null)
				return null;

			var parameters = new NameValueCollection
						{ 
							{ "access_token", result.AccessTokenValue },
							{ "user_id", "14926992" },
							{ "message", "Hello test"}
			};

			var sendMessageRequest = Nemiro.OAuth.OAuthUtility.ExecuteRequest
			(
				"GET",
				"https://api.vk.com/method/messages.send",
				parameters,
				null
			);

			return sendMessageRequest;
		}

		#endregion

		private WebBrowser mvWebBrowser;
		public WebBrowser WebBrowser { get { return mvWebBrowser; } set { mvWebBrowser = value; mvWebBrowser.LoadCompleted += mvWebBrowser_LoadCompleted; } }

		void mvWebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
		{
			try
			{
				AuthorizationResult result = OAuthWeb.VerifyAuthorizationAndRemoveRequest(e.Uri.ToString());
				IList<string> resultItems = new List<string>();

				RequestResult request = null;

				if (e.Uri.Query.IndexOf("code=") != -1 || e.Uri.Fragment.IndexOf("code=") != -1 || e.Uri.Query.IndexOf("oauth_verifier=") != -1)
				{
					if (result.IsSuccessfully)
					{
						//MessageBox.Show("my name: " + result.UserInfo.FullName);						

						request = GetDialogsRequest(result);
						foreach (UniValue item in request["response"])
						{
							if (item.ContainsKey("body"))
							{
								//message = String.Format("{0} | {1}", item["uid"], item["body"]);
								var message = String.Format("{0} | {1}", GetUserInfoRequest(item), item["body"]);

								Messages.Add(message);
							}
						}

						//request = SendMessageRequest(result);

					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
	}
}