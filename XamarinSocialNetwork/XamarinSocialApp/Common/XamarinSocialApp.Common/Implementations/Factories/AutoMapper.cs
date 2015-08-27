using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Common.Interfaces.Factories;

namespace XamarinSocialApp.Common.Implementations.Factories
{
	public class AutoMapper : IAutoMapper
	{
		private IObjectsByTypeFactory factory = new ObjectsByTypeFactory();

		public R GetObjectFromFactory<R>(object obj) where R : class
		{
			return factory.GetObjectFromFactory<R>(obj);
		}

		public R GetObjectFromFactory<T, R>() where R : class
		{
			return factory.GetObjectFromFactory<T, R>();
		}

		public T GetObjectFromFactory<T>()
		{
			return factory.GetObjectFromFactory<T>();
		}

		public void RegisterObject<T, R>(Func<T, R> func) where R : class
		{
			factory.RegisterObject<T, R>(func);
		}

		public void RegisterObject<T, R>(Func<R> func) where R : class
		{
			factory.RegisterObject<T, R>(func);
		}

		public void RegisterObject<T, R>() where R : new()
		{
			factory.RegisterObject<T, R>();
		}
	}
}
