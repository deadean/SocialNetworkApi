using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using XamarinSocialApp.Droid.Services.SQLiteConnector;
using XamarinSocialApp.UI.Services.Interfaces.SQLiteConnector;
using System.Threading.Tasks;
using XamarinSocialApp.UI.Services.Implementations.SQLiteConnector;
using SQLite.Net.Async;
using System.IO;
using SQLite.Net;

[assembly: Dependency(typeof(Android_SQLiteConnection))]

namespace XamarinSocialApp.Droid.Services.SQLiteConnector
{
	public class Android_SQLiteConnection : ISQLiteConnection
	{
		public async Task<ConnectionInfo<SQLiteAsyncConnection>> GetConnection()
		{
			ConnectionInfo<SQLiteAsyncConnection> connectionInfo = new ConnectionInfo<SQLiteAsyncConnection>();
			connectionInfo.IsInitializedDbStructure = true;
			var sqliteFilename = XamarinSocialApp.UI.Common.Implementations.Constants.Constants.Configuration.csLocalDbFileName;
			string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			var path = Path.Combine(documentsPath, sqliteFilename);

			if (!File.Exists(path))
			{
				var s = Forms.Context.Resources.OpenRawResource(Resource.Raw.talkmanager1);
				FileStream writeStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
				ReadWriteStream(s, writeStream);
				connectionInfo.IsInitializedDbStructure = false;
			}

			var plat = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var connectionFactory = new Func<SQLiteConnectionWithLock>(() =>
				new SQLiteConnectionWithLock(plat, new SQLiteConnectionString(path, storeDateTimeAsTicks: false)));
			var asyncConnection = new SQLiteAsyncConnection(connectionFactory);
			connectionInfo.Connection = asyncConnection;

			return connectionInfo;
		}

		void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int Length = 256;
			Byte[] buffer = new Byte[Length];
			int bytesRead = readStream.Read(buffer, 0, Length);
			while (bytesRead > 0)
			{
				writeStream.Write(buffer, 0, bytesRead);
				bytesRead = readStream.Read(buffer, 0, Length);
			}
			readStream.Close();
			writeStream.Close();
		}
	}
}