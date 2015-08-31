using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities;

namespace XamarinSocialApp.Services.Common.Interfaces.Cache
{
	public interface ICacheService
	{
		Task Add<T>(T item) where T : IEntity;
		Task Add<T>(IEnumerable<T> items) where T : IEntity;

		Task Remove<T>(T item) where T : IEntity;
		Task Remove<T>(IEnumerable<T> items) where T : IEntity;

		Task Update<T>(T item) where T : IEntity;
		Task Update<T>(IEnumerable<T> items) where T : IEntity;

		Task<IEnumerable<T>> Items<T>() where T : IEntity;
		Task<T> ItemById<T>(string id) where T : IEntity;
	}
}
