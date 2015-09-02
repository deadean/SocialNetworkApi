using Common.MVVM.Library;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.Services.UI.Interfaces.Web;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using XamarinSocialApp.UI.Data.Implementations.Entities.Databases;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class PageDialogWithFriendVm : AdvancedPageViewModelBase, IPageDialogWithFriendVm
	{

		#region Fields

		private readonly IApplicationWebService modIWebService;

		public ObservableCollection<DialogVm> Dialogs { get; set; }



		#endregion

		#region Commands


		#endregion

		#region Properties



		#endregion

		#region Ctor

		public PageDialogWithFriendVm()
			:this(ServiceLocator.Current.GetInstance<IApplicationWebService>())
		{
		}

		[PreferredConstructor]
		public PageDialogWithFriendVm(IApplicationWebService iWebService)
		{
			modIWebService = iWebService;
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods
		
		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			FriendsVm friend = navigationParameter as FriendsVm;
			if (friend == null)
				return;

			IUser user = new User() { FirstName = friend.FirstName, LastName = friend.LastName, SerializeInfo = friend.EntityModel.SerializeInfo };
 
			IsBusy = true;

			IEnumerable<IDialog> dialogs = await modIWebService.GetDialogWithFriend(user);
			this.Dialogs = new ObservableCollection<DialogVm>(dialogs.Select(x => new DialogVm(x)));
			this.OnPropertyChanged(x => x.Dialogs);

			IsBusy = false;
		}

		//public override async Task OnNavigatedTo(object navigationParameter)
		//{
		//	IUser user = navigationParameter as IUser;
		//	if (user == null)
		//		return;

		//	modUser = user;
		//	UserName = String.Format("{0} {1}", modUser.FirstName, modUser.LastName);

		//	IsBusy = true;

		//	IEnumerable<IDialog> dialogs = await modIWebService.GetDialogs(user);
		//	this.Dialogs = new ObservableCollection<DialogVm>(dialogs.Select(x => new DialogVm(x)));
		//	this.OnPropertyChanged(x => x.Dialogs);

		//	IsBusy = false;
		//}

		#endregion

		#region Command Execute Handlers


		#endregion



	}
}
