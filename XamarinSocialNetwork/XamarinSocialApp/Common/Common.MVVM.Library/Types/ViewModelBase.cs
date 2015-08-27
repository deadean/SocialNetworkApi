using XLabs.Forms.Mvvm;

namespace Common.MVVM.Library
{
	//using GalaSoft.MvvmLight;
	using System;
	using System.Diagnostics;
	using System.Threading;
	

	public interface INotifyPropertyChangedWithRaise
	{
		void RaisePropertyChanged1(string propertyName);
	}

	/// <summary>
	///   Base class for ViewModel pattern implementation
	/// </summary>
	public class ViewModelBaseCommon : XLabs.Forms.Mvvm.ViewModel, IViewModel, INotifyPropertyChangedWithRaise
	{
		#region Fields

		#endregion // Fields

		#region Properties

		#endregion // Properties

		#region Events

		#endregion // Events

		#region Public Methods

		public void RaisePropertyChanged1(string propertyName)
		{
			//this.RaisePropertyChanged(propertyName);
			this.NotifyPropertyChanged(propertyName);
			//OnPropertyChanged(propertyName);
		}

		public virtual void RefreshCommands() { }

		#endregion // Public Methods

		#region Protected Methods

		#endregion // Protected Methods
	}
}