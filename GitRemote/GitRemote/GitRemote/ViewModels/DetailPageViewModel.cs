using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Services;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Linq;
using GitRemote.GitHub.Managers;
using GitRemote.Views.MasterPageViews;

namespace GitRemote.ViewModels
{
    public class DetailPageViewModel : BindableBase, INavigationAware
    {
        private Session _session;
        private readonly INavigationService _navigationService;
        public DelegateCommand NotificationsCommand { get; }

        public DetailPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            NotificationsCommand = new DelegateCommand(OnNotificationsTapped);
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
        }

        private async void OnNotificationsTapped()
        {
            var navigationParameters = new NavigationParameters { { "Session", _session } };

            await _navigationService.NavigateAsync($"{nameof(NotificationsPage)}", navigationParameters);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( parameters.ContainsKey("Session") )
                _session = parameters["Session"] as Session;
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

    }
}
