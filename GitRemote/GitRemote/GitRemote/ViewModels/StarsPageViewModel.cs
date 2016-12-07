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

namespace GitRemote.ViewModels
{
    public class StarsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public NotifyTask<ObservableCollection<StarredRepositoryModel>> StarredRepositories { get; }
        private readonly StarredRepositoriesManager _starsManager;
        public DelegateCommand ItemTappedCommand { get; }
        public StarredRepositoryModel TappedItem { get; set; }

        public StarsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            ItemTappedCommand = new DelegateCommand(OnItemTapped);
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _starsManager = new StarredRepositoriesManager(_session);

            StarredRepositories = NotifyTask.Create(GetStarsAsync()); // executes async method and takes stars from it

            if ( StarredRepositories.IsFaulted )
                if ( StarredRepositories.Exception != null )
                    foreach ( var exception in StarredRepositories.Exception.InnerExceptions )
                        throw exception;


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

            if ( TappedItem.StarredRepositoryType == "Fork" )
                _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(ForkedRepositoryPage)}", parameters);
            else
                _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(PublicRepositoryPage)}", parameters);
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
