using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Common.Interfaces.Logging;
using XamarinSocialApp.Services.Common.Interfaces.FileSystem;
using XamarinSocialApp.Services.Common.Interfaces.Logging;

namespace XamarinSocialApp.Services.Common.Implementations.Logging
{
	public class LogService : ILogService
	{

		#region Fields

		private readonly IFileWorkerService modFileWorkerService;
		private readonly ILoggingSettings modLoggingSettings;
		private static ILogService modSingleton;

		#endregion

		#region Properties

		#endregion

		#region Ctor

		public LogService(IFileWorkerService fileService, ILoggingSettings settings)
		{
			modFileWorkerService = fileService;
			modLoggingSettings = settings;
		}

		#endregion

		#region Public Methods

		public static ILogService GetLogService()
		{
			if (modSingleton == null)
				modSingleton = 
					new LogService(ServiceLocator.Current.GetInstance<IFileWorkerService>(),
						ServiceLocator.Current.GetInstance<ILoggingSettings>());
			return modSingleton;
		}

		public async Task Write(string message)
		{
			try
			{
				string logMessage = string.Format(modLoggingSettings.LogFormat, DateTime.Now, message);
				await modFileWorkerService.AddToTextFileAsync(modLoggingSettings.LogFile, logMessage + Environment.NewLine);
			}
			catch (Exception ex)
			{

			}
		}

		public async Task Write(Exception exception)
		{
			try
			{
				string logMessage = string.Format(modLoggingSettings.LogFormat, DateTime.Now, exception.ToString());
				await modFileWorkerService.AddToTextFileAsync(modLoggingSettings.LogFile, logMessage + Environment.NewLine);
			}
			catch (Exception ex)
			{

			}
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

	}
}
