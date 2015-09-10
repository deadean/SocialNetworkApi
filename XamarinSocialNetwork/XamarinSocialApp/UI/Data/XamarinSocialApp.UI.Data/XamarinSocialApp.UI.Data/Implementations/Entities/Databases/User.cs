using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Common.Implementations.Factories;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.UI.Data.Implementations.Bases;

namespace XamarinSocialApp.UI.Data.Implementations.Entities.Databases
{
	public class User : AbstractEntityBase, IUser
	{
			
		#region Services

		#endregion

		#region Fields

		#endregion

		#region Properties

		[PrimaryKey]
		public string ID { get; set; }

		public string UserPhoto { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string SerializeInfo { get; set; }
		public string Uid { get; set; }
		public enSocialNetwork SocialNetwork { get; set; }

		#endregion

		#region Ctor

		public User()
		{
			ID = KeyGenerator.GetKey(ClassType);
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

		protected override string PrivateGetEntityId()
		{
			return this.ID;
		}

		protected override void GenerateId()
		{
			this.GenerateIdAndClassType(XamarinSocialApp.Data.Constants.Constants.Entitites.csUser);
		}

		#endregion
	}
}
