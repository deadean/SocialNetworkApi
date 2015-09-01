using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Data.Interfaces.Entities.Database
{
	public interface IDialog
	{
		IUser User { get; }
		IEnumerable<IMessage> Messages { get; }
	}
}
