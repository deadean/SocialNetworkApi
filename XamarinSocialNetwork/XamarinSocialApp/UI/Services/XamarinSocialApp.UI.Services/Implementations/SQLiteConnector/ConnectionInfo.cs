using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.UI.Services.Implementations.SQLiteConnector
{
	public class ConnectionInfo<T>
		where T : class
	{
		public T Connection { get; set; }
		public bool IsInitializedDbStructure { get; set; }
	}
}
