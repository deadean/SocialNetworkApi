namespace Common.MVVM.Library
{
	using System;
	using System.Windows.Input;

	public class RelayCommand : ICommand
	{
		#region Fields

		readonly Action<object> modExecute;
		readonly Predicate<object> modCanExecute;

		#endregion // Fields

		#region Constructors

		public RelayCommand(Action<object> execute)
			: this(execute, null)
		{
		}

		public RelayCommand(Action<object> execute, Predicate<object> canExecute)
		{
			if (execute == null)
				throw new ArgumentNullException("execute");

			modExecute = execute;
			modCanExecute = canExecute;
		}
		#endregion // Constructors

		#region ICommand Members

		public bool CanExecute(object parameter)
		{
			return modCanExecute == null || modCanExecute(parameter);
		}

		public void Execute(object parameter)
		{
			modExecute(parameter);
		}

		#endregion // ICommand Members


		public event EventHandler CanExecuteChanged;
	}
}
