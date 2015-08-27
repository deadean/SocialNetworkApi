using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Common.Interfaces.Factories
{
	public interface IObjectsByTypeFactory
	{
		R GetObjectFromFactory<R>(object obj)
			where R : class;

		R GetObjectFromFactory<T, R>()
			where R : class;

		T GetObjectFromFactory<T>();

		void RegisterObject<T, R>(Func<T, R> func)
			where R : class;

		void RegisterObject<T, R>(Func<R> func)
			where R : class;

		void RegisterObject<T, R>()
			where R : new();
	}
}
