using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Entities;

namespace XamarinSocialApp.Services.UI.Interfaces.InternalStorage
{
	public interface IInternalStorage
	{
		Task Save<T>(T item)
			where T : class, IEntity;

		Task Update<T>(T item)
			where T : class, IEntity;

		Task<List<T>> Items<T>()
			where T : class, IEntity;

		Task<T> ItemById<T>(string id)
			where T : class, IEntity;

		Task DeleteAsync<T>(T item)
			where T : class, IEntity;

		Task Initialize();


	}
}
