using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSocialApp.Services.UI.Interfaces.NavigationService;
using XamarinSocialApp.UI.Common.Interfaces.Views.Bases;

namespace XamarinSocialApp.UI.Common.Implementations.Bases
{
	public class BasePage : ContentPage, IBasePage
	{
		private INavigationServiceCommon mvNavigationService;
		public INavigationServiceCommon NavigationService
		{
			get
			{
				return mvNavigationService ?? (mvNavigationService = ServiceLocator.Current.GetInstance<INavigationServiceCommon>());
			}
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Run(async () => await NavigationService.OnAppearing(this.BindingContext));
		}

		protected async override void OnDisappearing()
		{
			await Task.Run(async () => await NavigationService.OnNavigatedFromAsync(this.BindingContext));
		}

		public async Task PostInitizlization()
		{
			
		}
	}
}
