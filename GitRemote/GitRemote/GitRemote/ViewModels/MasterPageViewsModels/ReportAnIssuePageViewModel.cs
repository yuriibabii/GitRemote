using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels.MasterPageViewsModels
{
    public class ReportAnIssuePageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        public ReportAnIssuePageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
