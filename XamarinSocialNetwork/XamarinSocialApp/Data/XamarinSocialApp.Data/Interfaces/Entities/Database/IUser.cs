using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Common.Interfaces.Entities;

namespace XamarinSocialApp.Data.Interfaces.Entities.Database
{
	public interface IUser : IEntity
	{
		string FirstName { get; set; }
		string LastName { get; set; }
		string SerializeInfo { get; set; }
		enSocialNetwork SocialNetwork { get; set; }
		string Uid { get; set; }
	}
}
