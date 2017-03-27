using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class StarsPageViewModel : BindableBase
    {
        private readonly Session _session;
        public NotifyTask<ObservableCollection<StarredRepositoryModel>> StarredRepositories { get; }
        private readonly StarredRepositoriesManager _starsManager;
        public DelegateCommand ItemTappedCommand { get; }
        public StarredRepositoryModel TappedItem { get; set; }

        public StarsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            ItemTappedCommand = new DelegateCommand(OnItemTapped);
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);

            _starsManager = new StarredRepositoriesManager(_session);

            StarredRepositories = NotifyTask.Create(GetStarsAsync());
        }

        private void OnItemTapped()
        {
            var ownerName = TappedItem.OwnerName;

            var reposName = TappedItem.StarredRepositoryName;

            var parameters = new NavigationParameters
            {
                { "OwnerName", ownerName},
                { "ReposName", reposName},
                { "Session", _session}
            };

            var path = TappedItem.StarredRepositoryType == "Fork"
                ? $"{nameof(ForkedRepositoryPage)}"
                : $"{nameof(PublicRepositoryPage)}";

            MessagingCenter.Send(new DoNavigationModel(path, parameters), DoNavigation);
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collection of stars</returns>
        private async Task<ObservableCollection<StarredRepositoryModel>> GetStarsAsync()
        {
            return new ObservableCollection<StarredRepositoryModel>
                (await _starsManager.GetStarsAsync());
        }
    }
}
