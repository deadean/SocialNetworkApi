using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.UI.Data.Implementations.Entities.Databases
{
	public class Message:IMessage
	{

		#region Services

		#endregion

		#region Fields

		#endregion

		#region Properties

		public string Content { get; set; }
		public IUser Sender { get; set; }
		public IUser Recipient { get; set; }
		public string ParticipientId { get; set; }
		public string MessageId { get; set; }

		#endregion

		#region Ctor

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion
	}
}
