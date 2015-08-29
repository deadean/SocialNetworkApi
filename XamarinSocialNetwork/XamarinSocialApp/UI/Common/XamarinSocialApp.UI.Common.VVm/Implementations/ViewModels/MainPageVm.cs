﻿using Common.MVVM.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;
using XamarinSocialApp.Services.UI.Interfaces.Web.OAuth;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class MainPageVm : AdvancedPageViewModelBase, IMainPageVm
	{

		#region Fields

		#endregion

		#region Services

		private readonly IOAuthService modOAuthService;

		#endregion

		#region Properties

		#endregion

		#region Commands

		public ICommand LoginCommand
		{
			get;
			private set;
		}

		public ICommand LoadMessagesCommand
		{
			get;
			private set;
		}
		
		#endregion

		#region Ctor

		public MainPageVm(IOAuthService oAuthService)
		{
			modOAuthService = oAuthService;
			LoginCommand = new AsyncCommand(OnLoginCommand);
			LoadMessagesCommand = new AsyncCommand(OnLoadMessagesCommad);
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion

		#region Command Execute Handlers

		private async Task OnLoginCommand()
		{
			try
			{
				IUser user = await modOAuthService.Login();
				await modNavigationService.Navigate<PageUserDialogsVm>(user);
			}
			catch (Exception ex)
			{
				//this.GetLog().Write(ex);
			}
		}

		private async Task OnLoadMessagesCommad()
		{
			try
			{
				await modOAuthService.GetDialogs();
			}
			catch (Exception ex)
			{

			}
		}

		#endregion

	}
}
