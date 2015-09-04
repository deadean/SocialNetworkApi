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
		public class MessageNewMyMessageWasSent : BaseSenderMessage<IMessage>
		{
			public MessageNewMyMessageWasSent(IMessage message)
				: base(message)
			{
			}
		}

		public class MessageNewMessageWasSentToMe : BaseSenderMessage<IMessage>
		{
			public MessageNewMessageWasSentToMe(IMessage message)
				: base(message)
			{
			}
		}
	}
}
