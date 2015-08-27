using GalaSoft.MvvmLight.Messaging;
using Nemiro.OAuth;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using vk2.VVms.ViewModels;

namespace vk2.VVms.Views
{
	/// <summary>
	/// Description for MainWindowView.
	/// </summary>
	public partial class MainWindowView : UserControl
	{
		public MainWindowView()
		{
			InitializeComponent();

			DataContextChanged += MainWindowView_DataContextChanged;
		}

		void MainWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			(e.NewValue as MainWindowVm).WebBrowser = wbVk;
		}

		//void wbVk_LoadCompleted(object sender, NavigationEventArgs e)
		//{
		//	try
		//	{
		//		AuthorizationResult result = OAuthWeb.VerifyAuthorizationAndRemoveRequest(e.Uri.ToString());
		//		IList<string> resultItems = new List<string>();

		//		RequestResult request = null;

		//		if (e.Uri.Query.IndexOf("code=") != -1 || e.Uri.Fragment.IndexOf("code=") != -1 || e.Uri.Query.IndexOf("oauth_verifier=") != -1)
		//		{
		//			if (result.IsSuccessfully)
		//			{
		//				//MessageBox.Show("my name: " + result.UserInfo.FullName);						

		//				//request = GetDialogsRequest(result);


		//				string message = String.Empty;

		//				//Messenger.Default.Send(AuthorizationResult);
		//				//foreach (UniValue item in request["response"])
		//				//{
		//				//	if (item.ContainsKey("body"))
		//				//	{
		//				//		//message = String.Format("{0} | {1}", item["uid"], item["body"]);
		//				//		message = String.Format("{0} | {1}", GetUserInfoRequest(item), item["body"]);

		//				//		resultItems.Items.Add(message);
		//				//	}
		//				//}
		//			}
		//		}

				
		//	}
		//	catch (Exception ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}
		//}
	}
}