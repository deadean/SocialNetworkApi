using SQLite.Net;
using SQLite.Net.Async;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using XamarinSocialApp.UI.Services.Implementations.SQLiteConnector;
using XamarinSocialApp.UI.Services.Interfaces.SQLiteConnector;
using XamarinSocialApp.WinRT.Tablet.Implementations.Services.SQLiteConnector;

[assembly: Dependency(typeof(WinRtTablet_SQLiteConnection))]

namespace XamarinSocialApp.WinRT.Tablet.Implementations.Services.SQLiteConnector
{
	public class WinRtTablet_SQLiteConnection : ISQLiteConnection
	{
		public async Task<ConnectionInfo<SQLiteAsyncConnection>> GetConnection()
		{
			ConnectionInfo<SQLiteAsyncConnection> connectionInfo = new ConnectionInfo<SQLiteAsyncConnection>();
			connectionInfo.IsInitializedDbStructure = true;

			var sqliteFilename = XamarinSocialApp.UI.Common.Implementations.Constants.Constants.Configuration.csLocalDbFileName;
			string path = Path.Combine(ApplicationData.Current.LocalFolder.Path, sqliteFilename);

			if (!await FileExists(path))
			{
				connectionInfo.IsInitializedDbStructure = false;
			}

			var plat = new SQLitePlatformWinRT();
			var connectionFactory = new Func<SQLiteConnectionWithLock>(() =>
				new SQLiteConnectionWithLock(plat, new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));
			var asyncConnection = new SQLiteAsyncConnection(connectionFactory);
			connectionInfo.Connection = asyncConnection;

			return connectionInfo;
		}

		private async Task<bool> FileExists(string fileName)
		{
			var result = false;
			try
			{
				var store = await ApplicationData.Current.LocalFolder.GetFilesAsync();
				result = store.Any(x => Path.GetFileName(x.Name) == Path.GetFileName(fileName));
			}
			catch (Exception ex)
			{
			}

			return result;
		}
	}
}
