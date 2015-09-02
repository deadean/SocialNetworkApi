using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;

namespace XamarinSocialApp.Data.Interfaces.Entities.OAuth.Vk
{
	public interface IVkUsers
	{
		IEnumerable<IUser> Users { get; set; }
	}
}
