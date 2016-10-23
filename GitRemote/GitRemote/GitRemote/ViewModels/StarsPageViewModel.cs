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
    public class StarsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public NotifyTask<ObservableCollection<StarredRepositoryModel>> StarredRepositories { get; }
        private readonly StarredRepositoriesManager _starsManager;

        public StarsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

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

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collention of stars</returns>
        private async Task<ObservableCollection<StarredRepositoryModel>> GetStarsAsync()
        {
            return new ObservableCollection<StarredRepositoryModel>
                (await _starsManager.GetStarsAsync());
        }
    }
}
