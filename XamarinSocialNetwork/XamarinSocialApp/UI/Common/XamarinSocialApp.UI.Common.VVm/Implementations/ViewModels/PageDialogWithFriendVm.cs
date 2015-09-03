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
using XamarinSocialApp.UI.Data.Implementations.Navigation;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class PageDialogWithFriendVm : AdvancedPageViewModelBase, IPageDialogWithFriendVm
	{

		#region Fields

		private readonly IApplicationWebService modIWebService;
		private IUser modUser;

		#endregion

		#region Commands


		#endregion

		#region Properties

		public ObservableCollection<MessageVm> Messages { get; set; }

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
			PageDialogWithFriendNavParams param = navigationParameter as PageDialogWithFriendNavParams;
			if (param.HasNotValue())
				return;

			modUser = param.User;

			if (modUser.HasNotValue())
				return;

			IsBusy = true;

			IDialog dialog = await modIWebService.GetDialogWithFriend(modUser, param.Friend);
			this.Messages = new ObservableCollection<MessageVm>(dialog.Messages.Select(x => new MessageVm(x)));
			this.OnPropertyChanged(x => x.Messages);

			IsBusy = false;
		}

		#endregion

		#region Command Execute Handlers


		#endregion



	}
}
