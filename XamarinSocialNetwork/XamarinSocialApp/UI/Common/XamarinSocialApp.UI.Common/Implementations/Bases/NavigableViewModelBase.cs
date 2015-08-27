using Common.MVVM.Library;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinSocialApp.Services.UI.Interfaces.NavigationService;

namespace XamarinSocialApp.UI.Common.Implementations.Bases
{
	public abstract class NavigableViewModelBase<T> : AdvancedViewModelBase<T>, INavigableAdvancedViewModelBase
	{
		#region Fields

		protected readonly INavigationServiceCommon modNavigationService;
		protected INavigationHelper modNavigationHelper;
		protected object modNavigationParameter;
		private bool mvIsLoading;
		private bool mvIsInitialized;

		#endregion

		#region Constructors

		public NavigableViewModelBase()
		{
			modNavigationService = ServiceLocator.Current.GetInstance<INavigationServiceCommon>();
		}

		#endregion

		#region Methods

		public void SetNavigationHelper(INavigationHelper navigationHelper)
		{
			modNavigationHelper = navigationHelper;
		}

		public async Task SetInitialized()
		{
			IsInitialized = true;
		}

		#endregion

		#region Abstract methods

		public virtual Task OnNavigatedTo(object navigationParameter)
		{
			return Task.Delay(0);
		}

		public virtual Task OnNavigatedBack()
		{
			return Task.Delay(0);
		}

		public virtual Task OnNavigatedFrom()
		{
			return this.CleanAsync();
		}

		#endregion

		#region Properties

		public bool IsInitialized
		{
			get
			{
				return mvIsInitialized;
			}
			private set
			{
				mvIsInitialized = value;
			}
		}

		public bool IsLoading
		{
			get
			{
				return mvIsLoading;
			}

			set
			{
				mvIsLoading = value;
				this.OnPropertyChanged();
			}
		}

		#endregion




	}

	public abstract class NavigableViewModelBase : NavigableViewModelBase<Object>
	{

	}
}
