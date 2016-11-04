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

namespace GitRemote.ViewModels.MasterPageViews
{
    public class GistsListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<GistModel>> Gists { get; set; }
        private readonly GistsManager _gistsManager;

        private string _tabTitle = string.Empty;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }

        public GistsListPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            if ( GistsManager.IsGitHubClient == false )
            {
                var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
                var session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
                GistsManager.SetGitHubClient(session);
            }

            _navigationService = navigationService;
            _gistsManager = new GistsManager();
            TabTitle = _gistsManager.GetTabTitle();
            Gists = NotifyTask.Create(GetGistsAsync());
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collection of stars</returns>
        private async Task<ObservableCollection<GistModel>> GetGistsAsync()
        {
            return new ObservableCollection<GistModel>
                (await _gistsManager.GetGistsAsync());
        }
    }
}
