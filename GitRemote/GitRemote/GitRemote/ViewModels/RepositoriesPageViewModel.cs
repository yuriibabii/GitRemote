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
    public class RepositoriesPageViewModel : BindableBase
    {
        private readonly Session _session;
        public NotifyTask<ObservableCollection<GroupingModel<string, RepositoryModel>>> GroupedRepositories { get; }
        private readonly RepositoriesManager _repositoriesManager;
        public DelegateCommand ItemTappedCommand { get; }
        public RepositoryModel TappedItem { get; set; }

        public RepositoriesPageViewModel(ISecuredDataProvider securedDataProvider)
        {
            ItemTappedCommand = new DelegateCommand(OnItemTapped);

            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);

            _repositoriesManager = new RepositoriesManager(_session);

            GroupedRepositories = NotifyTask.Create(GetRepositoriesAsync()); // executes async method and takes grouped repos from it
        }

        private void OnItemTapped()
        {
            var ownerName = TappedItem.OwnerName;

            var reposName = TappedItem.RepositoryName;

            var parameters = new NavigationParameters
            {
                { "OwnerName", ownerName},
                { "ReposName", reposName},
                { "Session", _session}
            };

            var path = TappedItem.RepositoryType == "Fork"
                ? $"{nameof(ForkedRepositoryPage)}"
                : $"{nameof(PublicRepositoryPage)}";

            MessagingCenter.Send(new DoNavigationModel(path, parameters), DoNavigation);
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collention of grouped repos</returns>
        private async Task<ObservableCollection<GroupingModel<string, RepositoryModel>>> GetRepositoriesAsync()
        {
            return new ObservableCollection<GroupingModel<string, RepositoryModel>>
                (await _repositoriesManager.GetRepositoriesAsync());
        }

    }
}
