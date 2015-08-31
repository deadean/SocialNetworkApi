using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;

namespace XamarinSocialApp.UI.Data.Implementations.Entities.OAuth
{
	public class User:IUser
	{
		#region Fields

		#endregion

		#region Properties

		public string Uid
		{
			get;
			private set;
		}

		public string FirstName
		{
			get;
			private set;
		}

		public string LastName
		{
			get;
			private set;
		}

		public string SerializeInfo
		{
			get;
			private set;
		}

		#endregion

		#region Ctor

		public User(string uid, string firstName, string lastName, string serializeInfo)
		{
			Uid = uid;
			FirstName = firstName;
			LastName = lastName;
			SerializeInfo = serializeInfo;
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion
		
	}
}
