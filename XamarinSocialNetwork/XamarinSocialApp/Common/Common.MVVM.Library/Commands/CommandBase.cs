using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Common.MVVM.Library
{
	public abstract class CommandBase : ViewModelBase, ICommand
	{
		#region Fields

		bool modIsExecuting;

		#endregion

		#region Properties

		public bool IsExecuting
		{
			get
			{
				return modIsExecuting;
			}

			set
			{
				modIsExecuting = value;
				RaisePropertyChanged();
			}
		}

		public Func<Task> BeforeCommandAsyncAction
		{
			private get;
			set;
		}

		public Func<Task> PostCommandAsyncAction
		{
			private get;
			set;
		}

		public bool IsCanExecute
		{
			get;
			protected set;
		}

		#endregion

		#region Events

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Abstract methods

		public abstract bool CanExecute(object parameter);

		public async void Execute(object parameter)
		{
			if (IsExecuting)
				return;

			IsExecuting = true;

			if (BeforeCommandAsyncAction != null)
			{
				await BeforeCommandAsyncAction();
			}

			await ExecuteProtected(parameter);

			if (PostCommandAsyncAction != null)
			{
				await PostCommandAsyncAction();
			}

			IsExecuting = false;
		}

		#endregion

		#region Public methods

		public void RaiseCanExecuteChanged()
		{
			var handler = CanExecuteChanged;

			if (handler != null)
			{
				handler(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Protected

		protected abstract Task ExecuteProtected(object parameter);

		#endregion

		#region Overriden

		public override void Cleanup()
		{
			base.Cleanup();
			BeforeCommandAsyncAction = null;
			PostCommandAsyncAction = null;
		}

		#endregion
	}
}
