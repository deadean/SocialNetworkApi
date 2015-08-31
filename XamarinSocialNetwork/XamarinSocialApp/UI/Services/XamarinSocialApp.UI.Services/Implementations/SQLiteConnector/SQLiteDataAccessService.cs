using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities;
using XamarinSocialApp.Services.Common.Interfaces.DataBases;
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

		public SQLiteDataAccessService(ISQLiteConnection connection)
		{
			modConnectionInfo = connection;
		}

		#endregion

		#region Public Methods

		public async Task Save<T>(T item) where T : IEntity, new()
		{
			await modConnection.InsertAsync(item);
			//await modConnection.UpdateWithChildrenAsync(item);
		}

		public async Task Update<T>(T item) where T : IEntity, new()
		{
			//await modConnection.InsertOrReplaceWithChildrenAsync(item);
			await modConnection.UpdateWithChildrenAsync(item);
		}

		public Task DeleteAsync<T>(T item) where T : IEntity, new()
		{
			return modConnection.DeleteAsync(item);
		}

		public async Task<List<T>> Items<T>() where T : IEntity, new()
		{
			var res = await modConnection.GetAllWithChildrenAsync<T>(null, true);
			return res;
		}

		public Task<T> ItemById<T>(string id) where T : IEntity, new()
		{
			return modConnection.FindAsync<T>(id);
		}

		public async Task ResetItems<T>() where T : IEntity, new()
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

			//await modConnection.DropTableAsync<PhotoCommentRelation>();

			//await modConnection.CreateTableAsync<SynchronizationInfo>();
		}

		#endregion

		#region Private Methods

		#endregion



	}
}
