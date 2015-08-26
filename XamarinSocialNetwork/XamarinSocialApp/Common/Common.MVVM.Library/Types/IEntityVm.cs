using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MVVM.Library
{
	public interface IEntityVm
	{
	}

	public interface IEntityVm<T> :IEntityVm
	{
		T Entity { get; }
	}
}
