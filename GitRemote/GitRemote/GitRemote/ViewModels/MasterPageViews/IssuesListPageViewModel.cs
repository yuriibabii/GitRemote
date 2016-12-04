using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
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
    public class IssuesListPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<IssueModel>> Issues { get; set; }
        private readonly IssueDashboardManager _issuesManager;

        public IssuesListPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            if ( IssueDashboardManager.IsGitHubClient == false )
            {
                var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
                var session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
                IssueDashboardManager.SetGitHubClient(session);
            }

            _navigationService = navigationService;
            _issuesManager = new IssueDashboardManager();
            Issues = NotifyTask.Create(GetIssuesAsync());
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collection of issues</returns>
        private async Task<ObservableCollection<IssueModel>> GetIssuesAsync()
        {
            return new ObservableCollection<IssueModel>
                (await _issuesManager.GetIssuesAsync());
        }
    }
}
