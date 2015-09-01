using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Common.Interfaces.Factories;
using XamarinSocialApp.Data.Common.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.Cache;
using XamarinSocialApp.Services.UI.Interfaces.InternalStorage;
using XamarinSocialApp.Services.UI.Interfaces.Model;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;

namespace XamarinSocialApp.UI.Services.Implementations.Model
{
	public class InternalModelService : IInternalModelService
	{

		#region Fields

		private readonly IInternalStorage modStorage;
		private readonly IObjectsByTypeFactory modFactoryObjects;
		private readonly ICacheService modCacheService;
		private bool modIsInitialized;

		#endregion

		#region Properties

		#endregion

		#region Ctor

		public InternalModelService(
			IInternalStorage storage
			, IObjectsByTypeFactory factory
			, ICacheService cacheService
			)
		{
			modStorage = storage;
			modFactoryObjects = factory;
			modCacheService = cacheService;

			InitObjectsFactory();
		}

		#endregion

		#region Public Methods

		public async Task Initialize()
		{
			if (modIsInitialized)
				return;

			await modStorage.Initialize();
			await InitializeDefaultData();
			modIsInitialized = true;
		}

		public async Task SaveEntity<T>(T item) where T : class, IEntity
		{
			//item.WithType<IVersionedEntity>(x => x.Date = DateTime.Now);

			await modCacheService.Add<T>(item);

			await modStorage.Save<T>(item);
		}

		public async Task UpdateEntityAsync<T>(T item) where T : class, IEntity
		{
			//item.WithType<IVersionedEntity>(x => x.Date = DateTime.Now);
			await modStorage.Update<T>(item);
			await modCacheService.Update<T>(item);
		}

		public Task<T> CreateEntity<T>() where T : IEntity
		{
			return Task.Run(() => modFactoryObjects.GetObjectFromFactory<T>());
		}

		public async Task DeleteEntityAsync<T>(T item) where T : class, IEntity
		{
			await modStorage.DeleteAsync<T>(item);
			await modCacheService.Remove<T>(item);
		}

		public Task<T> ItemById<T>(string id) where T : class, IEntity
		{
			return modCacheService.ItemById<T>(id);
		}

		public Task<IEnumerable<T>> Items<T>() where T : class, IEntity
		{
			return modCacheService.Items<T>();
		}

		#endregion

		#region Private Methods

		private void InitObjectsFactory()
		{
			//modFactoryObjects.RegisterObject<IPhoto, Photo>();
			//modFactoryObjects.RegisterObject<IPhotoCommentRelation, PhotoCommentRelation>();
			//modFactoryObjects.RegisterObject<IComment, PhotoComment>();
			//modFactoryObjects.RegisterObject<IPreference, Preference>();
			//modFactoryObjects.RegisterObject<IAlbum, Album>();
			//modFactoryObjects.RegisterObject<IDestinationSource, DestinationSource>();
			//modFactoryObjects.RegisterObject<IAlbumDestinationSourceRelation, AlbumDestinationSourceRelation>();
			//modFactoryObjects.RegisterObject<ISynchronizationInfo, SynchronizationInfo>();
		}

		private async Task InitializeDefaultData()
		{
			try
			{
				var users = await modStorage.Items<User>();
				users.With(async x => await modCacheService.Add<User>(users));
			}
			catch (Exception ex)
			{

			}
		}

		#endregion


	}
}
