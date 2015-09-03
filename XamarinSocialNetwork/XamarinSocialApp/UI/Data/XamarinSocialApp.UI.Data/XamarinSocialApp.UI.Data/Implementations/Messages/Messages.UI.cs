using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Interfaces.Messages;

namespace XamarinSocialApp.UI.Data.Implementations.Messages
{
	public static class Messages
	{
		public class MessageNewMessageWasSent : BaseSenderMessage<IMessage>
		{
			public MessageNewMessageWasSent(IMessage message)
				: base(message)
			{
			}
		}
	}
}
