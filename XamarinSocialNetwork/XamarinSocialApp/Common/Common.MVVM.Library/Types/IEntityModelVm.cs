using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MVVM.Library
{
	public interface IEntityModelVm
	{
	}

	public interface IEntityModelVm<T> : IEntityModelVm
	{
		T EntityModel { get; set; }

		Task Retarget(T newTarget);
	}
}
