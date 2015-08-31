using SQLite.Net.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.UI.Services.Implementations.SQLiteConnector;

namespace XamarinSocialApp.UI.Services.Interfaces.SQLiteConnector
{
	public interface ISQLiteConnection
	{
		Task<ConnectionInfo<SQLiteAsyncConnection>> GetConnection();
	}
}
