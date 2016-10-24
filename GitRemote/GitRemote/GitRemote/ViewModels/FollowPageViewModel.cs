using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GitRemote.ViewModels
{
    public class FollowPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public NotifyTask<ObservableCollection<FollowModel>> Follows { get; }
        private readonly FollowManager _followManager;

        public FollowPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());

            var session = new Session(UserManager.GetLastUser(), store.Properties.First().Value,
                store.Properties["PrivateFeedUrl"]);

            var navigationParameters = new NavigationParameters { { "Session", session } };

            _followManager = new FollowManager(session);

            Follows = NotifyTask.Create(GetFollowsAsync());
        }

        private async Task<ObservableCollection<FollowModel>> GetFollowsAsync()
        {
            return new ObservableCollection<FollowModel>
                (await _followManager.GetFollowsAsync());
        }
    }
}
