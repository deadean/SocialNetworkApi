using Common.MVVM.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XamarinSocialApp.UI.Common.Implementations.Bases
{
	public abstract class AdvancedPageViewModelBase : NavigableViewModelBase
	{
		#region Fields

		private readonly AsyncCommand mvBackCommand;

		#endregion

		#region Ctor

		protected AdvancedPageViewModelBase()
		{
			mvBackCommand = new AsyncCommand(OnBackCommand);
		}

		#endregion

		#region Public Methods

		#endregion

		#region Protected Methods

		#endregion

		#region Properties

		#endregion

		#region Commands

		public AsyncCommand BackCommand
		{
			get { return mvBackCommand; }
		}

		#endregion

		#region Commands Execute Handlers

		private Task OnBackCommand()
		{
			return Task.Run(() => modNavigationHelper.GoBackCommand.Execute(null));
		}

		#endregion
	}

	public abstract class AdvancedPageViewModelBase<T> : AdvancedPageViewModelBase
	{
		private T mvEntityModel;

		public T EntityModel
		{
			get
			{
				return mvEntityModel;
			}

			set
			{
				mvEntityModel = value;
				this.OnPropertyChanged();
			}
		}
	}
}
