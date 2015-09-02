using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using XamarinSocialApp.Common.Implementations.Factories;
using XamarinSocialApp.Common.Interfaces.Factories;
using XamarinSocialApp.Data.Common.Interfaces.Logging;
using XamarinSocialApp.Services.Common.Implementations.Logging;
using XamarinSocialApp.Services.Common.Interfaces.Cache;
using XamarinSocialApp.Services.Common.Interfaces.DataBases;
using XamarinSocialApp.Services.Common.Interfaces.FileSystem;
using XamarinSocialApp.Services.Common.Interfaces.Logging;
using XamarinSocialApp.Services.UI.Interfaces.InternalStorage;
using XamarinSocialApp.Services.UI.Interfaces.Model;
using XamarinSocialApp.Services.UI.Interfaces.NavigationService;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using XamarinSocialApp.UI.Common.Views.Implementations.Views;
using XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using XamarinSocialApp.UI.Services.Implementations.Cache;
using XamarinSocialApp.UI.Services.Implementations.InternalStorage;
using XamarinSocialApp.UI.Services.Implementations.Model;
using XamarinSocialApp.UI.Services.Implementations.NavigationService;
using XamarinSocialApp.UI.Services.Implementations.SQLiteConnector;
using XamarinSocialApp.UI.Services.Implementations.Web;
using XamarinSocialApp.UI.Services.Interfaces.SQLiteConnector;
using XLabs.Forms.Mvvm;
using XLabs.Forms.Services;
using XLabs.Ioc;
using XLabs.Platform.Services;

namespace XamarinSocialApp
{
	public class App : Application
	{
		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Ctor

		public App()
		{
			try
			{
				if (!SimpleIoc.Default.IsRegistered<INavigationServiceCommon>())
				{
					SimpleIoc.Default.Register<INavigationServiceCommon, NavigationServiceXLabs>();
					SimpleIoc.Default.Register<INavigationService>(() => { return Resolver.Resolve<INavigationService>(); });

					ConfigureDependencyResolves();
					ConfigureNavigationService();
				}

				Initialize();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		private async void Initialize(object extraContent = null)
		{
			Page mainPage = null;
			if (MainPage == null)
			{
				mainPage = new MainPage();

				SimpleIoc.Default.GetInstance<INavigationServiceCommon>().NavigationService = 
					new NavigationService(mainPage.Navigation);
			}

			var vm = ServiceLocator.Current.GetInstance<MainPageVm>();

			if (extraContent != null)
			{
				if (MainPage == null)
				{
					mainPage.BindingContext = vm;
					MainPage = new NavigationPage(mainPage);
				}

				return;
			}

			mainPage.BindingContext = vm;

			

			MainPage = new NavigationPage(mainPage);
		}

		private void ConfigureDependencyResolves()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			ConfigureDependenciesByPlatform();

			ConfigureDependenciesByDependencyService();

			ConfigureDependencies();

			ConfigureDependencyBlocks();

			ConfigureLocatorStrategies();

			ConfigureDependenciesByViewModels();

		}

		private void ConfigureLocatorStrategies()
		{
		}

		private static void ConfigureDependencyBlocks()
		{
		}

		private static void ConfigureDependencies()
		{
			//SimpleIoc.Default.Register<ILoggingSettings, LogSettings>();
			SimpleIoc.Default.Register<ILogService, LogService>();
			SimpleIoc.Default.Register<IObjectsByTypeFactory, ObjectsByTypeFactory>();

			SimpleIoc.Default.Register<ISQLiteDataAccessService, SQLiteDataAccessService>();
			SimpleIoc.Default.Register<ICacheService, CacheService>();
			SimpleIoc.Default.Register<IInternalStorage, InternalSQLiteStorage>();
			SimpleIoc.Default.Register<IInternalModelService, InternalModelService>();
			SimpleIoc.Default.Register<IApplicationWebService, ApplicationWebService>();


			ConfigureWebServiceDependencies();
			ConfigureAutoMapper();
		}

		private static void ConfigureWebServiceDependencies()
		{
		}

		private static void ConfigureAutoMapper()
		{
			try
			{
				IAutoMapper mapper = new AutoMapper();

				SimpleIoc.Default.Register(() => mapper);
			}
			catch (Exception ex)
			{

			}
		}

		private static void ConfigureDependenciesByDependencyService()
		{
			try
			{
				SimpleIoc.Default.Register(() => DependencyService.Get<IFileWorkerService>());
				SimpleIoc.Default.Register(() => DependencyService.Get<IOAuthService>());
				SimpleIoc.Default.Register(() => DependencyService.Get<ISQLiteConnection>());
			}
			catch (Exception ex)
			{
			}

		}

		private static void ConfigureDependenciesByViewModels()
		{
			SimpleIoc.Default.Register<MainPageVm>();
		}

		private static void ConfigureDependenciesByPlatform()
		{
		}

		#region Navigation Mapping

		private void ConfigureNavigationService()
		{
			ViewFactory.EnableCache = true;
			ViewFactory.Register<MainPage, MainPageVm>();
			ViewFactory.Register<PageUserDialogs, PageUserDialogsVm>();
			ViewFactory.Register<PageUserFriends, PageUserFriendsVm>(); 

			ViewFactory.Register<PageDialogWithFriend, PageDialogWithFriendVm>();

			ConfigureNavigationServiceByPlatform();
		}

		private void ConfigureNavigationServiceByPlatform()
		{
			Device.OnPlatform
			(
				Android: ConfigureNavigationServiceByAndroid,
				Default: ConfigureNavigationServiceByDefault
			);

			if (Device.OS == TargetPlatform.Windows)
			{
				ConfigureNavigationServiceByDefault();
			}
		}

		private void ConfigureNavigationServiceByAndroid()
		{
		}

		private void ConfigureNavigationServiceByDefault()
		{
		}

		#endregion

		#endregion

		#region Protected Methods

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

		#endregion

	}
}
