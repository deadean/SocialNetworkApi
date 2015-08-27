using Common.MVVM.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLabs.Platform.Services;

namespace XamarinSocialApp.Services.UI.Interfaces.NavigationService
{
	public interface INavigationServiceCommon
	{
		INavigationService NavigationService { get; set; }

		Task NavigateToRoot();

		Task Navigate<TVm>(object parameter = null, bool isRetarget = false, bool isFromCache = true)
			where TVm : class, INavigableAdvancedViewModelBase;

		Task NavigateBackAsync();

		Task OnAppearing(object sender);

		Task OnNavigatedFromAsync(object p);
	}

	public interface INavigationServiceCommon<T> : INavigationServiceCommon
	{
		void SetNavigationContext(T navigationContext);
	}
}
