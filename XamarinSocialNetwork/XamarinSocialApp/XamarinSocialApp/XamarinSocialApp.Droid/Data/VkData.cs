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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace XamarinSocialApp.Droid.Data
{
	public static class VkData
	{
		public class VkUsers
		{
			public User[] response { get; set; }
		}

		public class VkDialogsResponse
		{
			[JsonProperty("response")]
			public VkDialogsItems Response { get; set; }
		}

		public class VkMessageByIdResponse
		{
			[JsonProperty("response")]
			public VkMessageByIdResponseItems Response { get; set; }
		}

		public class VkMessagesResponse
		{
			[JsonProperty("response")]
			public List<object> Response { get; set; }
		}

		public class VkLongPoolServerResponse
		{
			[JsonProperty("response")]
			public LongPoolServer Response { get; set; }
		}

		public class VkLongPoolServerUpdates
		{
			[JsonProperty("ts")]
			public string Ts { get; set; }

			[JsonProperty("updates")]
			public IEnumerable<JArray> Updates { get; set; }
		}

		public class VkDialogsItems
		{
			[JsonProperty("count")]
			public int Count { get; set; }

			[JsonProperty("items")]
			public IEnumerable<MessageItem> Messages { get; set; }
		}

		public class VkMessageByIdResponseItems
		{
			[JsonProperty("count")]
			public int Count { get; set; }

			[JsonProperty("items")]
			public IEnumerable<MessageByIdResponseItem> Messages { get; set; }
		}

		public class VkMessagesItem
		{
			public MessageInDialog Message { get; set; }
		}


		public class User
		{
			public string uid { get; set; }
			public string photo_50 { get; set; }
			public string first_name { get; set; }
			public string last_name { get; set; }
		}

		public class Message
		{
			[JsonProperty("user_id")]
			public string UserId { get; set; }

			[JsonProperty("body")]
			public string Body { get; set; }
		}

		public class MessageInDialog
		{
			[JsonProperty("mid")]
			public string MessageId { get; set; }

			[JsonProperty("uid")]
			public string UserId { get; set; }

			[JsonProperty("body")]
			public string Body { get; set; }
		}

		public class MessageItem
		{
			[JsonProperty("message")]
			public Message Message { get; set; }
		}

		public class MessageByIdResponseItem
		{
			[JsonProperty("out")]
			public string Out { get; set; }
		}

		public class LongPoolServer
		{
			[JsonProperty("key")]
			public string Key { get; set; }
			[JsonProperty("server")]
			public string ServerUrl { get; set; }
			[JsonProperty("ts")]
			public string Ts { get; set; }
		}

		public class LongPoolServerUpdate
		{
			public object[] Values { get; set; }
		}
	}
}