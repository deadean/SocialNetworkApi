using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.OAuth;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using Common.MVVM.Library;

namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public sealed class PageUserDialogsVm : AdvancedPageViewModelBase, IPageUserDialogsVm
	{
		#region Fields

		private IUser modUser;
		private string mvUserName;
		private IList<string> mvItems;

		#endregion

		#region Properties

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

		public IList<string> Items
		{
			get
			{
				return mvItems;
			}

			set
			{
				mvItems = value;
				this.OnPropertyChanged();
			}
		}
		

		#endregion

		#region Ctor

		#endregion

		#region Public Methods

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		public override async Task OnNavigatedTo(object navigationParameter)
		{
			IList<string> tempList = navigationParameter as IList<string>;

			if (tempList == null)
				return;

			UserName = tempList[0];
			tempList.RemoveAt(0);

			Items = tempList;
		}

		#endregion

	}
}
