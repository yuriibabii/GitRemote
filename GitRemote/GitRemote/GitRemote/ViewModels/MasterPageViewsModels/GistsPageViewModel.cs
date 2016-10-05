using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels.MasterPageViewsModels
{
    public class GistsPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;

        public GistsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            
        }
    }
}
