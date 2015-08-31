using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Cache;
using XamarinSocialApp.Data.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.Cache;

namespace XamarinSocialApp.UI.Services.Implementations.Cache
{
	public class CacheService : ICacheService
	{

		#region Fields

		private Dictionary<Type, List<IEntity>> modContainer = new Dictionary<Type, List<IEntity>>();

		#endregion

		#region Properties

		#endregion

		#region Ctor

		#endregion

		#region Public Methods

		public async Task Add<T>(T item) where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				modContainer[typeof(T)].Add(item);
				return;
			}

			modContainer.Add(typeof(T), new List<IEntity>() { item });
		}

		public async Task Add<T>(IEnumerable<T> items) where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				modContainer[typeof(T)].AddRange(items.Cast<IEntity>());
				return;
			}

			modContainer.Add(typeof(T), items.Cast<IEntity>().ToList());
		}

		public async Task Remove<T>(T item) where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				var cacheableItems = modContainer.Values.Select(x => x.OfType<ICacheableItem>());
				foreach (var cacheableItemsValues in cacheableItems)
				{
					foreach (var cacheableItem in cacheableItemsValues)
					{
						await cacheableItem.RemoveRelatedData(item);
					}
				}

				modContainer[typeof(T)].RemoveAll(x => x.IdEntity == item.IdEntity);
				return;
			}

			throw new KeyNotFoundException("Cannot be updated not exist in cache item");
		}

		public Task Remove<T>(IEnumerable<T> items) where T : IEntity
		{
			throw new NotImplementedException();
		}

		public async Task Update<T>(T item) where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				var itemInCache = modContainer[typeof(T)].FirstOrDefault(x => x.IdEntity == item.IdEntity);
				itemInCache = item;
				return;
			}

			throw new KeyNotFoundException("Cannot be updated not exist in cache item");
		}

		public Task Update<T>(IEnumerable<T> items) where T : IEntity
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<T>> Items<T>() where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				return modContainer[typeof(T)].Cast<T>();
			}

			return default(IEnumerable<T>);
		}

		public async Task<T> ItemById<T>(string id) where T : IEntity
		{
			if (modContainer.ContainsKey(typeof(T)))
			{
				return (T)modContainer[typeof(T)]
					.FirstOrDefault(x => x.IdEntity == id);
			}

			return default(T);
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

	}
}
