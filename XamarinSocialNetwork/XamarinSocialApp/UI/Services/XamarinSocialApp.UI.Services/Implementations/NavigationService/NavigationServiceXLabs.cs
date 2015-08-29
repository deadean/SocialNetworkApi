using Common.MVVM.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinSocialApp.Services.UI.Interfaces.NavigationService;
using XamarinSocialApp.UI.Common.Interfaces.Views.Bases;
using XLabs.Forms.Mvvm;
using XLabs.Platform.Services;

namespace XamarinSocialApp.UI.Services.Implementations.NavigationService
{
	public class NavigationServiceXLabs : INavigationServiceCommon
	{

		#region Fields

		private INavigationService _navigationService;

		#endregion

		#region Properties

		public INavigationService NavigationService
		{
			get
			{
				return _navigationService;
			}

			set
			{
				_navigationService = value;
			}
		}

		#endregion

		#region Ctor

		public NavigationServiceXLabs(INavigationService navService)
		{
			_navigationService = navService;
		}

		#endregion

		#region Public Methods

		private async Task NavigateTo<T>(
			object parameter = null,
			bool animated = true,
			bool isRemoveCurrentPage = false,
			bool isRetarget = false,
			bool isFromCache = true
			)
			where T : class
		{
			try
			{
				Page currentPage = null;
				if (isRemoveCurrentPage)
				{
					currentPage = await GetCurrentPage();
				}

				ExecuteNavigate<T>(parameter, animated, isFromCache);

				await Task.Factory.StartNew(async () =>
				{
					var vm = GetCurrentPageVm();

					if (vm != null)
					{
						if (!vm.IsInitialized || isRetarget)
						{
							await vm.OnNavigatedTo(parameter);
							await vm.SetInitialized();

							IBasePage page = (IBasePage)await GetCurrentPage();
							await page.PostInitizlization();
						}

						if (isRemoveCurrentPage)
						{
							await RemoveCurrentPage(currentPage);
						}
					}
				});
			}
			catch (Exception ex)
			{
			}

		}

		public Task OnAppearing(object sender)
		{
			var vm = sender as INavigableAdvancedViewModelBase;

			if (vm == null)
				return Task.FromResult<object>(null);

			return vm.OnNavigatedBack();
		}

		public Task OnNavigatedFromAsync(object sender)
		{
			var vm = sender as INavigableAdvancedViewModelBase;

			if (vm == null)
				return Task.FromResult<object>(null);

			return vm.OnNavigatedFrom();
		}

		public Task NavigateBackAsync()
		{
			return Application.Current.MainPage.Navigation.PopAsync();
		}

		public Task Navigate<TVm>(object parameter, bool isRetarget = false, bool isFromCache = true) where TVm : class, INavigableAdvancedViewModelBase
		{
			return this.NavigateTo<TVm>(parameter, isRetarget: isRetarget, isFromCache: isFromCache);
		}

		public async Task NavigateToRoot()
		{
			var page = await GetCurrentPage();
			await page.Navigation.PopToRootAsync();
		}

		#endregion

		#region Private Methods

		private Task RemoveCurrentPage(Page page)
		{
			var navigation = Application.Current.MainPage.Navigation;
			navigation.RemovePage(page);
			return Task.FromResult<object>(null);
		}

		private Task<Page> GetCurrentPage()
		{
			var navigation = Application.Current.MainPage.Navigation;

			if (!navigation.NavigationStack.Any())
				return Task.FromResult<Page>(null);

			var page = navigation.NavigationStack.Last();

			return Task.FromResult<Page>(page);
		}

		private INavigableAdvancedViewModelBase GetCurrentPageVm()
		{
			var navigation = Application.Current.MainPage.Navigation;

			if (!navigation.NavigationStack.Any())
				return null;

			var page = navigation.NavigationStack.Last();
			var vm = page.BindingContext as INavigableAdvancedViewModelBase;
			return vm;
		}

		private void ExecuteNavigate<T>(object parameter, bool animated, bool isFromCache) where T : class
		{
			if (!isFromCache)
			{
				ViewFactory.EnableCache = false;
			}
			
			_navigationService.NavigateTo<T>(animated, parameter);

			if (!isFromCache)
			{
				ViewFactory.EnableCache = true;
			}
		}

		#endregion

		#region Protected Methods

		#endregion

	}
}
