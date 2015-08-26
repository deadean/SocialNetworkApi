namespace Common.MVVM.Library
{
	/// <summary>
	///   Defines 
	/// </summary>
	public interface IAdvancedViewModel : IViewModel
	{
		#region Properties

		bool IsCancelPending { get; }

		bool IsEnabled { get; set; }

		#endregion // Properties

		#region Methods

		void Clean();

		void Interrupt();

		void Refresh();

		#endregion // Methods
	}
}