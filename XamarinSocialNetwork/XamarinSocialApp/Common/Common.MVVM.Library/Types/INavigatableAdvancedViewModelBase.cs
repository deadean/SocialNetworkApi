using System.Threading.Tasks;
namespace Common.MVVM.Library
{
	public interface INavigableAdvancedViewModelBase : IViewModel
	{
		Task OnNavigatedTo(object navigationParameter);
		Task OnNavigatedBack();
		Task OnNavigatedFrom();
		Task SetInitialized();
		void SetNavigationHelper(INavigationHelper navigationHelper);

		bool IsLoading { get; }
		bool IsInitialized { get; }
		
	}
}
