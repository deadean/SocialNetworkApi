using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Entities;

namespace XamarinSocialApp.Data.Common.Interfaces.Cache
{
	public interface ICacheableItem : IEntity
	{
		Task RemoveRelatedData(IEntity item);
	}
}
