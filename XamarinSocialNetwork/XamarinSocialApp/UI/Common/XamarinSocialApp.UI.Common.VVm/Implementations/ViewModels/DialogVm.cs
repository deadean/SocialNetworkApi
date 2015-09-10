using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Data.Interfaces.Entities.Database;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using Common.MVVM.Library;


namespace XamarinSocialApp.UI.Common.VVm.Implementations.ViewModels
{
	public class DialogVm : AdvancedPageViewModelBase<IDialog>
	{

		#region Services

		#endregion

		#region Fields

		private string mvUserPhoto;
		private string mvName;
		private string mvContent;

		#endregion

		#region Properties

		public string UserPhoto
		{
			get
			{
				return mvUserPhoto;
			}

			set
			{
				mvUserPhoto = value;
				this.OnPropertyChanged();
			}
		}

		public string Name
		{
			get
			{
				return mvName;
			}

			set
			{
				mvName = value;
				this.OnPropertyChanged();
			}
		}


		public string Content
		{
			get
			{
				return mvContent;
			}

			set
			{
				mvContent = value;
				this.OnPropertyChanged();
			}
		}
		
		

		#endregion

		#region Ctor

		public DialogVm(IDialog dialog)
		{
			EntityModel = dialog;
			UserPhoto = EntityModel.User.UserPhoto;
			UserPhoto = dialog.User.UserPhoto;
			Name = String.Format("{0} {1}", dialog.User.FirstName, dialog.User.LastName);
			Content = EntityModel.Messages.First().Content;
			IsBusy = true;
		}

		#endregion

		#region Commands

		#endregion

		#region Commands Execute Handlers

		#endregion

		#region Public Methods

		public void UpdateUserInfo(IUser user)
		{
			if (user.HasNotValue())
				return;

			EntityModel.User.FirstName = user.FirstName;
			EntityModel.User.LastName = user.LastName;
			EntityModel.User.Uid = user.Uid;
			Name = String.Format("{0} {1}", EntityModel.User.FirstName, EntityModel.User.LastName);
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		#endregion
	}
}
