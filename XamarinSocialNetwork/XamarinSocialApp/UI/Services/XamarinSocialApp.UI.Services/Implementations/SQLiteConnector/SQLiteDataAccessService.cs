using SQLite.Net.Async;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSocialApp.Data.Common.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.DataBases;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using XamarinSocialApp.UI.Services.Interfaces.SQLiteConnector;

namespace XamarinSocialApp.UI.Services.Implementations.SQLiteConnector
{
	public class SQLiteDataAccessService : ISQLiteDataAccessService
	{
		#region Fields

		private SQLiteAsyncConnection modConnection;
		private readonly ISQLiteConnection modConnectionInfo;

		#endregion

		#region Ctor

		public SQLiteDataAccessService()
		{
			modConnectionInfo = DependencyService.Get<ISQLiteConnection>();
		}

		#endregion

		#region Public Methods

		public async Task Save<T>(T item) where T : class, IEntity
		{
			await modConnection.InsertAsync(item);
			//await modConnection.UpdateWithChildrenAsync(item);
		}

		public async Task Update<T>(T item) where T : class, IEntity
		{
			//await modConnection.InsertOrReplaceWithChildrenAsync(item);
			await modConnection.UpdateWithChildrenAsync(item);
		}

		public Task DeleteAsync<T>(T item) where T : class, IEntity
		{
			return modConnection.DeleteAsync(item);
		}

		public async Task<List<T>> Items<T>() where T : class, IEntity
		{
			var res = await modConnection.GetAllWithChildrenAsync<T>(null, true);
			return res;
		}

		public Task<T> ItemById<T>(string id) where T : class, IEntity
		{
			return modConnection.FindAsync<T>(id);
		}

		public async Task ResetItems<T>() where T : class, IEntity
		{
			await modConnection.DropTableAsync<T>();
			await modConnection.CreateTableAsync<T>();
		}

		public async Task Initialize()
		{
			ConnectionInfo<SQLiteAsyncConnection> connectionInfo = await modConnectionInfo.GetConnection();
			modConnection = connectionInfo.Connection;

			if (connectionInfo.IsInitializedDbStructure)
				return;

			await modConnection.DropTableAsync<User>();

			await modConnection.CreateTableAsync<User>();
		}

		#endregion

		#region Private Methods

		#endregion



	}
}
