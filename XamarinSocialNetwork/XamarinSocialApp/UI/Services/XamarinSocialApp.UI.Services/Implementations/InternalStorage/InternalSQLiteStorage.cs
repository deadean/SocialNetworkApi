using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.DataBases;
using XamarinSocialApp.Services.UI.Interfaces.InternalStorage;

namespace XamarinSocialApp.UI.Services.Implementations.InternalStorage
{
	public sealed class InternalSQLiteStorage : IInternalStorage
	{

		#region Fields

		private ISQLiteDataAccessService modDataAccessService;
		//private Dictionary<Type, List<IEntity>> modCache;

		#endregion

		#region Properties

		#endregion

		#region Ctor

		public InternalSQLiteStorage(ISQLiteDataAccessService connection)
		{
			modDataAccessService = connection;
			//modCache = new Dictionary<Type, List<IEntity>>();
		}

		#endregion

		#region Public Methods

		public async Task Save<T>(T item) where T : class, IEntity
		{
			await modDataAccessService.Save<T>(item);
		}

		public async Task Update<T>(T item) where T : class, IEntity
		{
			await modDataAccessService.Update<T>(item);
		}

		public Task DeleteAsync<T>(T item) where T : class, IEntity
		{
			return modDataAccessService.DeleteAsync<T>(item);
		}

		public async Task<List<T>> Items<T>() where T : class, IEntity
		{
			//if (modCache.ContainsKey(typeof(T)))
			//	return modCache[typeof(T)].Cast<T>().ToList();

			//var res = await modDataAccessService.Items<T>();
			//modCache.Add(typeof(T), res.Cast<IEntity>().ToList());

			//return res;
			return await modDataAccessService.Items<T>();
		}

		public Task<T> ItemById<T>(string id) where T : class, IEntity
		{
			return modDataAccessService.ItemById<T>(id);
		}

		#endregion


		public Task Initialize()
		{
			return modDataAccessService.Initialize();
		}

	}
}
