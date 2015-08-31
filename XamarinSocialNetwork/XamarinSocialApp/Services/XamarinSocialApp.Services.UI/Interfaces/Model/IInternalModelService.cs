using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities;

namespace XamarinSocialApp.Services.UI.Interfaces.Model
{
	public interface IInternalModelService
	{
		Task SaveEntity<T>(T item) where T : class, IEntity;
		Task UpdateEntityAsync<T>(T item) where T : class, IEntity;
		Task DeleteEntityAsync<T>(T item) where T : class, IEntity;
		Task<T> ItemById<T>(string id) where T : class, IEntity;
		Task<IEnumerable<T>> Items<T>() where T : class, IEntity;
		Task<T> CreateEntity<T>() where T : IEntity;
		Task Initialize();
	}
}
