using Nemiro.OAuth;
using Nemiro.OAuth.Clients;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using Nemiro.OAuth.LoginForms;

namespace vk2
{
	enum enVkApi
	{
		getAllMess,
		getAllDialogs
	}


	public partial class MainWindow : Window
	{

		private Dictionary<enVkApi, string> modApi
			= new Dictionary<enVkApi, string>();

		//private int appId = 5030035;
		//private int scope = (int)(VkontakteScopeList.audio | VkontakteScopeList.friends | VkontakteScopeList.messages);

		public MainWindow()
		{
			InitializeComponent();

			modApi.Add(enVkApi.getAllDialogs, "https://api.vk.com/method/messages.getDialogs");

			//wbVk.Navigate(OAuthWeb.GetAuthorizationUrl( String.Format("http://api.vkontakte.ru/oauth/authorize?client_id={0}&scope={1}&display=popup&response_type=token", appId, scope));

			RegisterClient();

			wbVk.LoadCompleted +=wbVk_LoadCompleted;
		}

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
					Scope = "status,friends,messages"
				}
			);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			wbVk.Navigate(OAuthWeb.GetAuthorizationUrl((sender as Button).Tag.ToString()));
		}

		void wbVk_LoadCompleted(object sender, NavigationEventArgs e)
		{
			try
			{
				AuthorizationResult result = OAuthWeb.VerifyAuthorizationAndRemoveRequest(e.Uri.ToString());
				RequestResult request = null;

				if (e.Uri.Query.IndexOf("code=") != -1 || e.Uri.Fragment.IndexOf("code=") != -1 || e.Uri.Query.IndexOf("oauth_verifier=") != -1)
				{
					if (result.IsSuccessfully)
					{
						//MessageBox.Show("my name: " + result.UserInfo.FullName);						

						request = ExecuteRequest(result);

						string message = String.Empty;
						foreach (UniValue item in request["response"])
						{
							if (item.ContainsKey("body"))
							{
								message = String.Format("{0}", item["body"]);
							}
							lbMessages.Items.Add(message);
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private static RequestResult ExecuteRequest(AuthorizationResult result)
		{
			if (result == null)
				return null;

			//var parameters = new NameValueCollection
			//			{ 
			//				{ "access_token", result.AccessTokenValue },
			//				{ "count", "10" }
			//};

			var parameters2 = new NameValueCollection
						{ 
							{ "access_token", result.AccessTokenValue },
							{ "user_id", "14926992" },
							{ "message", "Hello test"}
			};

			//var getMessagesRequest = Nemiro.OAuth.OAuthUtility.ExecuteRequest
			//(
			//	"GET",
			//	"https://api.vk.com/method/messages.getDialogs",
			//	parameters,
			//	null
			//);

			var sendMessageRequest = Nemiro.OAuth.OAuthUtility.ExecuteRequest
			(
				"GET",
				"https://api.vk.com/method/messages.send",
				parameters2,
				null
			);

			return sendMessageRequest;
			//return getMessagesRequest;
		}
	}
}
