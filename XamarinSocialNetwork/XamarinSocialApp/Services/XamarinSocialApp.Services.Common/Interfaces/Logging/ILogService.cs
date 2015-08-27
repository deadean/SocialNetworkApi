using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.Services.Common.Interfaces.Logging
{
	public interface ILogService
	{
		Task Write(string message);
		Task Write(Exception exception);
	}
}
