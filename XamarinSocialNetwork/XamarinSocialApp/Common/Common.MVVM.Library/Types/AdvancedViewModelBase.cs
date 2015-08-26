namespace Common.MVVM.Library
{
	using Microsoft.Practices.ServiceLocation;
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;

	/// <summary>
	///   Represents 
	/// </summary>
	public abstract class AdvancedViewModelBase : ViewModelBaseCommon
	{
		#region Fields

		//private ILogService modLogService;

		#endregion // Fields

		#region Ctor

		protected AdvancedViewModelBase()
			: this(false)
		{

		}

		protected AdvancedViewModelBase(bool isEnabled)
		{
			//modLogService = ServiceLocator.Current.GetInstance<ILogService>();
		}

		#endregion // Ctor

		#region Properties

		#endregion // Properties

		#region Commands

		public DelegateCommand RefreshCommand { get; private set; }

		#endregion // Commands

		#region Command Can Execute Handlers

		#endregion // Command Can Execute Handlers

		#region Command Execute Handlers

		protected virtual async Task RefreshPrivateAsync() { }

		protected virtual async Task CleanPrivateAsync() { }

		#endregion // Command Execute Handlers

		#region Public Methods

		public async Task RefreshAsync()
		{
			try
			{
				await RefreshPrivateAsync();
			}
			catch (Exception)
			{
			}
			finally
			{
			}
		}

		public async Task CleanAsync()
		{
			try
			{
				await CleanPrivateAsync();
			}
			catch (Exception)
			{
			}
			finally
			{
			}
		}

		//public ILogService GetLog()
		//{
		//	return modLogService;
		//}

		#endregion // Public Methods

		#region Protected Methods

		protected virtual void CleanPrivate()
		{

		}


		#endregion // Protected Methods

		#region Methods Override

		/// <summary>
		///   Refreshes UI Command Elements IsEnabled state
		/// </summary>
		public override void RefreshCommands()
		{
			base.RefreshCommands();
		}

		#endregion // Methods Override
	}

	public abstract class AdvancedViewModelBase<T> : AdvancedViewModelBase, IEntityModelVm<T>
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

		public virtual Task Retarget(T newTarget) { EntityModel = newTarget; return RetargetPrivate(newTarget); }

		protected virtual async Task RetargetPrivate(T newTarget) { }
	}
}