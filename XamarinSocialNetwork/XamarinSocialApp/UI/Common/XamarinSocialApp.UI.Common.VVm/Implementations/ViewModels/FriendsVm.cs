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
	public class FriendsVm : AdvancedPageViewModelBase<IUser>
	{

		#region Services

		#endregion

		#region Fields

		private string mvUserPhoto;
		private string mvFirstName;
		private string mvLastName;

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

		public string FirstName
		{
			get
			{
				return mvFirstName;
			}

			set
			{
				mvFirstName = value;
				this.OnPropertyChanged();
			}
		}

		public string LastName
		{
			get
			{
				return mvLastName;
			}

			set
			{
				mvLastName = value;
				this.OnPropertyChanged();
			}
		}

		#endregion


		#region Ctor

		public FriendsVm(IUser user)
		{
			EntityModel = user;
			UserPhoto = EntityModel.UserPhoto;
			FirstName = EntityModel.FirstName;
			LastName = EntityModel.LastName;
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

		#endregion


	}
}
