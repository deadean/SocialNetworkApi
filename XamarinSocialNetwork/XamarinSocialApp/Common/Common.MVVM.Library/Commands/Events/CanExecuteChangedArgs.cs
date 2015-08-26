using System;

namespace Common.MVVM.Library
{
	public class CanExecuteChangedArgs : EventArgs
	{
		#region Constructor

		public CanExecuteChangedArgs(bool canExecuteChanged)
		{
			CanExecute = canExecuteChanged;
		}

		#endregion

		#region Properties

		public bool CanExecute { get; private set; }

		#endregion
	}
}
