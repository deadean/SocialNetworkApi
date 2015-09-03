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
using XamarinSocialApp.UI.Data.Implementations.Navigation;
using GalaSoft.MvvmLight.Messaging;
using XamarinSocialApp.Data.Interfaces.Messages;
using MessagesUI = XamarinSocialApp.UI.Data.Implementations.Messages.Messages;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public sealed class PageUserDialogsVm : AdvancedPageViewModelBase, IPageUserDialogsVm
	{

		#region Fields

		private IUser modUser;
		private string mvUserName;
		private DialogVm mvSelectedDialog;

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

		public DialogVm SelectedDialogVm
		{
			get
			{
				return mvSelectedDialog;
			}

			set
			{
				if (mvSelectedDialog == value)
					return;

				mvSelectedDialog = value;
				this.OnPropertyChanged();
				if (mvSelectedDialog.HasNotValue())
					return;

				IDialog selectedDialog = value.EntityModel;
				SelectedDialogVm = null;
				var navParam = new PageDialogWithFriendNavParams(modUser, selectedDialog.User);
				modNavigationService.Navigate<PageDialogWithFriendVm>(navParam, isFromCache: false);
			}
		}
		
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

		public IUser User
		{
			get { return modUser; }
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

				for (int i = 0; i < this.Dialogs.Count-groupLength; i += groupLength)
				{
					sw.Start();

					IUser user1 = await GetUserInfo(i);
					IUser user2 = await GetUserInfo(i+1);
					IUser user3 = await GetUserInfo(i+2);

					this.Dialogs[i].UpdateUserInfo(user1);
					this.Dialogs[i+1].UpdateUserInfo(user2);
					this.Dialogs[i+2].UpdateUserInfo(user3);

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

		private async Task<IUser> GetUserInfo(int i)
		{
			IUser user = null;
			while (true)
			{
				try
				{
					user = await this.modIWebService.GetUserInfoRequest(this.Dialogs[i].EntityModel.User);
				}
				catch (Exception ex)
				{

				}

				if (user.HasValue())
					break;
			}
			return user;
		}

		private void OnNewMessageWasSent(MessagesUI.MessageNewMessageWasSent message)
		{
			try
			{
				var dialogRecipient = this.Dialogs.First(x => x.EntityModel.User.Uid == message.Sender.Recipient.Uid);
				if (dialogRecipient.HasNotValue())
					return;

				dialogRecipient.Content = message.Sender.Content;
			}
			catch (Exception ex)
			{

			}
		}
		
		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			try
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

				Messenger.Default.Register<MessagesUI.MessageNewMessageWasSent>(this, OnNewMessageWasSent);
			}
			catch (Exception ex)
			{

			}
		}

		#endregion

		#region Command Execute Handlers

		private async Task OnShowUserFriendsCommand()
		{
			try
			{
				await modNavigationService.Navigate<PageUserFriendsVm>(modUser, isFromCache: false);
			}
			catch (Exception ex)
			{

			}
		}

		#endregion

	}
}
