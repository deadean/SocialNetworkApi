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


namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public sealed class PageUserDialogsVm : AdvancedPageViewModelBase, IPageUserDialogsVm
	{
		#region Fields

		private IUser modUser;
		private string mvUserName;

		private readonly IApplicationWebService modIWebService;


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
		}

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods
		
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
		}

		#endregion

	}
}
