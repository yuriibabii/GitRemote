using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels.MasterMenuPage
{
    public class IssueDashboardPageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        public IssueDashboardPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
