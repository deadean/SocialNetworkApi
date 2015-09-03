using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSocialApp.UI.Common.Implementations.Bases;
using XamarinSocialApp.UI.Common.Interfaces.ViewModels;
using XamarinSocialApp.UI.Common.Interfaces.Views.Bases;

namespace XamarinSocialApp.UI.Common.Views.Implementations.Views
{
	public partial class PageUserDialogs : ContentPage, IBasePage
	{

		#region Fields

		#endregion

		#region Properties

		#endregion

		#region Ctor

		public PageUserDialogs()
		{
			InitializeComponent();
		}

		public PageUserDialogs(object p)
			: this()
		{

		}

		#endregion

		#region Public Methods

		public void OnToolbarNewClicked(object sender, EventArgs args)
		{
			this.BindingContext.WithType<IPageUserDialogsVm>(x=>x.ShowUserFriendsCommand.Execute(null));
		}

		#endregion

		#region Private Methods

		#endregion

		#region Protected Methods

		public async Task PostInitizlization()
		{
		}

		#endregion
		
	}
}
