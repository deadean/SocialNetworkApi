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
		private IUser modUser;

		#endregion

		#region Commands


		#endregion

		#region Properties

		public ObservableCollection<DialogVm> Dialogs { get; set; }

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
			modUser = navigationParameter as IUser;

			if (modUser.HasNotValue())
				return;

			IsBusy = true;

			IEnumerable<IDialog> dialogs = await modIWebService.GetDialogWithFriend(modUser);
			this.Dialogs = new ObservableCollection<DialogVm>(dialogs.Select(x => new DialogVm(x)));
			this.OnPropertyChanged(x => x.Dialogs);

			IsBusy = false;
		}

		#endregion

		#region Command Execute Handlers


		#endregion



	}
}
