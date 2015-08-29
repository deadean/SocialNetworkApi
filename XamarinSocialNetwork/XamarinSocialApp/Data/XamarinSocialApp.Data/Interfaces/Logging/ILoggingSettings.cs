using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Data.Common.Interfaces.Logging
{
	public interface ILoggingSettings
	{
		string LogFormat { get; }

		string LogFile { get; }
	}
}
