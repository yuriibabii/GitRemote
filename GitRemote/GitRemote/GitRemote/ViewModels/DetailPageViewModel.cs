using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Linq;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

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

            //MessagingCenter.Subscribe<DoNavigationModel>(this, DoNavigation, OnDoNavigation);
        }

        private async void OnDoNavigation(DoNavigationModel model)
        {
            MessagingCenter.Unsubscribe<DoNavigationModel>(this, DoNavigation);

            await _navigationService.NavigateAsync(model.Path, model.Parameters);
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

            MessagingCenter.Subscribe<DoNavigationModel>(this, DoNavigation, OnDoNavigation);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<DoNavigationModel>(this, DoNavigation);
        }

    }
}
