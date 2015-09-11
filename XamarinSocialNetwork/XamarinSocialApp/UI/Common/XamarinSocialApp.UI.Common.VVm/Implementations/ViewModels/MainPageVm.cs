using Common.MVVM.Library;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinSocialApp.Data.Common.Enums;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Model;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class MainPageVm : AdvancedPageViewModelBase, IMainPageVm
	{

		#region Fields

		#endregion

		#region Services

		private readonly IApplicationWebService modIWebService;
		private readonly IInternalModelService modIInternalService;

		#endregion

		#region Properties

		#endregion

		#region Commands

		public ICommand LoginVkCommand
		{
			get;
			private set;
		}

		public ICommand LoginTwitterCommand
		{
			get;
			private set;
		}

		#endregion

		#region Ctor

		public MainPageVm(IApplicationWebService iWebService, IInternalModelService iService)
		{
			modIWebService = iWebService;
			modIInternalService = iService;
			LoginVkCommand = new AsyncCommand(OnLoginVkCommand);
			LoginTwitterCommand = new AsyncCommand(OnLoginTwitterCommand);
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		protected override async Task RefreshPrivateAsync()
		{
			if (!IsInitialized)
			{
				await ServiceLocator.Current.GetInstance<IInternalModelService>().Initialize();
			}
		}

		public async override Task OnNavigatedBack()
		{
			if (IsInitialized)
				return;

			await Task.Run(async () =>
			{
				await RefreshAsync();
				await SetInitialized();
			});
		}

		#endregion

		#region Command Execute Handlers

		private async Task OnLoginVkCommand()
		{
			try
			{
				var users = await modIInternalService.Items<User>();
				IUser user = users == null ? null : users.FirstOrDefault();

				if (user == null)
				{
					user = await modIWebService.Login(enSocialNetwork.VK);

					if (user == null)
						return;

					await modIInternalService.SaveEntity<User>(user as User);
				}
				await modNavigationService.Navigate<PageUserDialogsVm>(user, isFromCache: true);
			}
			catch (Exception ex)
			{
				//this.GetLog().Write(ex);
			}
		}

		private async Task OnLoginTwitterCommand()
		{
			IUser user = null;
			try
			{
					user = await modIWebService.Login(enSocialNetwork.Twitter);

					if (user == null)
						return;

					//await modIInternalService.SaveEntity<User>(user as User);
				
				//await modNavigationService.Navigate<PageUserDialogsVm>(user, isFromCache: true);
			}
			catch (Exception ex)
			{
				//this.GetLog().Write(ex);
			}
		}

		#endregion

	}
}
