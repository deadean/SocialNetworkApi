using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Data.Interfaces.Entities.Database
{
	public interface IUser : IEntity
	{
		string FirstName { get; set; }
		string LastName { get; set; }
	}
}
