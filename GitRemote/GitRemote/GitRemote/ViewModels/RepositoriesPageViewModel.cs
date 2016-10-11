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
    public class RepositoriesPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public NotifyTask<ObservableCollection<RepositoryModel>> Repositories { get; }
        private readonly RepositoriesManager _repositoriesManager;

        public RepositoriesPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _repositoriesManager = new RepositoriesManager(_session);

            Repositories = NotifyTask.Create(GetRepositoriesAsync());

            if ( Repositories.Exception != null )
                foreach ( var exception in Repositories.Exception.InnerExceptions )
                    throw exception;
        }

        private async Task<ObservableCollection<RepositoryModel>> GetRepositoriesAsync()
        {
            return new ObservableCollection<RepositoryModel>(await _repositoriesManager.GetRepositoriesAsync());
        }

    }
}
