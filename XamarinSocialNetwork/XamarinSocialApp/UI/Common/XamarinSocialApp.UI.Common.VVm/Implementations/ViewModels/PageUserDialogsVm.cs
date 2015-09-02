using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using Common.MVVM.Library;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using System.Collections.ObjectModel;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using Common.MVVM.Library;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;
using System.Windows.Input;


namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public sealed class PageUserDialogsVm : AdvancedPageViewModelBase, IPageUserDialogsVm
	{
		#region Fields

		private IUser modUser;
		private string mvUserName;

		private readonly IApplicationWebService modIWebService;

		#endregion

		#region Commands

		public ICommand ShowUserFriendsCommand
		{
			get;
			private set;
		}

		#endregion

		#region Properties

		public ObservableCollection<DialogVm> Dialogs { get; set; }
		
		public string UserName
		{
			get
			{
				return mvUserName;
			}

			set
			{
				mvUserName = value;
				this.OnPropertyChanged();
			}
		}

		#endregion

		#region Ctor

		public PageUserDialogsVm()
			:this(ServiceLocator.Current.GetInstance<IApplicationWebService>())
		{
		}

		[PreferredConstructor]
		public PageUserDialogsVm(IApplicationWebService iWebService)
		{
			modIWebService = iWebService;
			ShowUserFriendsCommand = new AsyncCommand(OnShowUserFriendsCommand);
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		private async Task LoadUserDialogInfo()
		{
			try
			{
				int groupLength = 3;
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				TimeSpan ts = new TimeSpan(0,0,0,1000);

				for (int i = 0; i < this.Dialogs.Count; i += groupLength)
				{
					sw.Start();

					this.Dialogs[i].UpdateUserInfo(await this.modIWebService.GetUserInfoRequest(this.Dialogs[i].EntityModel.User));
					this.Dialogs[i+1].UpdateUserInfo(await this.modIWebService.GetUserInfoRequest(this.Dialogs[i+1].EntityModel.User));
					this.Dialogs[i+2].UpdateUserInfo(await this.modIWebService.GetUserInfoRequest(this.Dialogs[i+2].EntityModel.User));
					this.Dialogs[i].IsBusy = false;
					this.Dialogs[i+1].IsBusy = false;
					this.Dialogs[i+2].IsBusy = false;

					await Task.Delay(800);
				}
			}
			catch (Exception ex)
			{

			}
		}
		
		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			IUser user = navigationParameter as IUser;
			if (user == null)
				return;

			modUser = user;
			UserName = String.Format("{0} {1}", modUser.FirstName, modUser.LastName);

			IsBusy = true;

			IEnumerable<IDialog> dialogs = await modIWebService.GetDialogs(user);
			this.Dialogs = new ObservableCollection<DialogVm>(dialogs.Select(x => new DialogVm(x)));
			this.OnPropertyChanged(x => x.Dialogs);

			IsBusy = false;

			this.LoadUserDialogInfo();
		}

		#endregion

		#region Command Execute Handlers

		private async Task OnShowUserFriendsCommand()
		{
			try
			{
				IEnumerable<IUser> friends = await modIWebService.ShowUserFriends(modUser);

				await modNavigationService.Navigate<PageUserFriendsVm>(friends, isFromCache: false);
			}
			catch (Exception ex)
			{

			}
		}

		#endregion

	}
}
