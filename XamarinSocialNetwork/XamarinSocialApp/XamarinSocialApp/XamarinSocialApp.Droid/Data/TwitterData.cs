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

namespace XamarinSocialApp.Droid.Data
{
	public static class TwitterData
	{
		public class TwitterUser
		{
			public string ConsumerKey {get;set;}
			public string ConsumerSecret {get;set;}
			public string OAuthToken {get;set;}
			public string OAuthTokenSecret {get;set;}
			public string ScreenName {get;set;}
			public ulong UserID {get;set;}

			public TwitterUser()
			{

			}
		}
	}
}