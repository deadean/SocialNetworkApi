using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.UI.Data.Implementations.Entities.Databases
{
	public class Dialog : IDialog
	{

		#region Services

		#endregion

		#region Fields

		#endregion

		#region Properties

		public IUser User { get; private set; }
		public IEnumerable<IMessage> Messages { get; private set; }

		#endregion

		#region Ctor

		public Dialog(IUser user, IEnumerable<IMessage> messages)
		{
			User = user;
			Messages = messages;
		}

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
