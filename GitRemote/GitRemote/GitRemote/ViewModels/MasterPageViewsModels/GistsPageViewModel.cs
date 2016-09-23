using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels.MasterPageViewsModels
{
    public class GistsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        public GistsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
