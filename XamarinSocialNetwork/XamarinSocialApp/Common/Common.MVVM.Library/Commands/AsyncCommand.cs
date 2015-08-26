using GalaSoft.MvvmLight;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Common.MVVM.Library
{
	public class AsyncCommand : CommandBase
	{
		#region Fields

		Func<bool> modCanExecutePredicate;
		Func<Task> modAsyncHandler;

		#endregion

		#region Constructors

		public AsyncCommand(Func<Task> asyncHandler, Func<bool> canExecutePredicate)
		{
			modCanExecutePredicate = canExecutePredicate;
			modAsyncHandler = asyncHandler;
		}

		public AsyncCommand(Func<Task> asyncHandler)
		{
			modAsyncHandler = asyncHandler;
		}

		#endregion

		#region Overrided methods

		public override bool CanExecute(object parameter)
		{
			return modCanExecutePredicate == null || modCanExecutePredicate();
		}

		protected override async Task ExecuteProtected(object parameter)
		{
			if (modAsyncHandler != null)
			{
				await modAsyncHandler();
			}
		}

		#endregion
	}

	public class AsyncCommand<T> : CommandBase
	{
		#region Fields

		Func<T, bool> modCanExecutePredicate;
		Func<T, Task> modAsyncHandler;

		#endregion

		#region Constructors

		public AsyncCommand(Func<T, Task> asyncHandler, Func<T, bool> canExecutePreditcate)
		{
			modCanExecutePredicate = canExecutePreditcate;
			modAsyncHandler = asyncHandler;
		}

		public AsyncCommand(Func<T, Task> asyncHandler)
		{
			modAsyncHandler = asyncHandler;
		}

		#endregion

		#region Overrided methods

		public override bool CanExecute(object parameter)
		{
			return modCanExecutePredicate == null || modCanExecutePredicate((T)parameter);
		}

		protected override async Task ExecuteProtected(object parameter)
		{
			if (modAsyncHandler != null)
			{
				await modAsyncHandler((T)parameter);
			}
		}

		#endregion
	}
}
