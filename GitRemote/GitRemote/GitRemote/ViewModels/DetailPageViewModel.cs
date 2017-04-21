using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Linq;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;

namespace GitRemote.ViewModels
{
    public class DetailPageViewModel : BindableBase, INavigationAware
    {
        private Session _session;
        private readonly INavigationService _navigationService;
        public DelegateCommand NotificationsCommand { get; }
        private readonly IEventAggregator _eventAggregator;

        public DetailPageViewModel(INavigationService navigationService,
            ISecuredDataProvider securedDataProvider,
            IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            NotificationsCommand = new DelegateCommand(OnNotificationsTapped);
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            _eventAggregator = eventAggregator;

            _eventAggregator
                .GetEvent<MessageService.DoNavigation>()
                .Subscribe(OnDoNavigation, false);
        }

        private void OnDoNavigation(DoNavigationModel model)
        {
            _navigationService.NavigateAsync(model.Path, model.Parameters);
        }

        private async void OnNotificationsTapped()
        {
            var navigationParameters = new NavigationParameters { { nameof(Session), _session } };

            await _navigationService.NavigateAsync($"{nameof(NotificationsPage)}", navigationParameters);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( parameters.ContainsKey(nameof(Session)) )
                _session = parameters[nameof(Session)] as Session;
        }

        public void OnNavigatingTo(NavigationParameters parameters) { }

        public void OnNavigatedFrom(NavigationParameters parameters) { }
    }
}
