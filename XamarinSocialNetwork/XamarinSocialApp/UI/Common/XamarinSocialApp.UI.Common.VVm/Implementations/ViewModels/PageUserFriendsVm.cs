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


namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class PageUserFriendsVm : AdvancedPageViewModelBase, IPageUserFriendsVm
	{
		#region Services

		#endregion

		#region Fields

		private readonly IApplicationWebService modIWebService;

		public ObservableCollection<FriendsVm> Friends { get; set; }
		
		private FriendsVm mvSelectedFriend;

		private IUser modUser;

		#endregion

		#region Properties

		public FriendsVm SelectedFriend
		{
			get
			{
				return mvSelectedFriend;
			}
			set
			{
				mvSelectedFriend = value;
				this.OnPropertyChanged();

				if (mvSelectedFriend == null)
					return;

				var navParam = new PageDialogWithFriendNavParams(modUser, mvSelectedFriend.EntityModel);
				modNavigationService.Navigate<PageDialogWithFriendVm>(navParam, isFromCache: false);
			}
		}

		#endregion

		#region Ctor

		public PageUserFriendsVm()
			: this(ServiceLocator.Current.GetInstance<IApplicationWebService>())
		{
			
		}

		[PreferredConstructor]
		public PageUserFriendsVm(IApplicationWebService iWebService)
		{
			modIWebService = iWebService;
		}

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			modUser = navigationParameter as IUser;
			if (modUser.HasNotValue())
				return;

			IEnumerable<IUser> friends = await modIWebService.GetUserFriends(modUser);
			if (friends == null)
				return;

			IsBusy = true;

			this.Friends = new ObservableCollection<FriendsVm>(friends.Select(x => new FriendsVm(x)));
			this.OnPropertyChanged(x => x.Friends);

			IsBusy = false;
		}

		#endregion
	}
}
