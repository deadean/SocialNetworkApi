using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Enums;

namespace XamarinSocialApp.Data.Interfaces.Entities.OAuth
{
	public interface IUser
	{
		string Uid { get; }
		string FirstName { get; }
		string LastName { get; }
		string SerializeInfo { get; }
		enSocialNetwork SocialNetwork { get; }
	}
}
