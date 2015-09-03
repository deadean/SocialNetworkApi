using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Data.Interfaces.Messages
{
	public class BaseSenderMessage
	{
	}

	public class BaseSenderMessage<T> : BaseSenderMessage
	{
		public BaseSenderMessage(T sender)
		{
			Sender = sender;
		}
		public T Sender { get; set; }
	}

	public class BaseSenderEnumarableMessage<R> : BaseSenderMessage<IEnumerable<R>>
	{
		public BaseSenderEnumarableMessage(IEnumerable<R> sender)
			: base(sender)
		{

		}
	}
}
