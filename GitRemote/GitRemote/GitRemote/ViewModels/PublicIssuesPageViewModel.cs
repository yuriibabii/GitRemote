using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GitRemote.ViewModels
{
    public class PublicIssuesPageViewModel
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<IssueModel>> Issues { get; set; }
        private readonly PublicIssuesManager _publicIssuesManager;

        public PublicIssuesPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            var session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            _publicIssuesManager = new PublicIssuesManager(session);
            Issues = NotifyTask.Create(GetPublicIssuesAsync("gitrem2", "NormalRepos"));
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collection of issues</returns>
        private async Task<ObservableCollection<IssueModel>> GetPublicIssuesAsync(string ownerName, string reposName)
        {
            return new ObservableCollection<IssueModel>
                (await _publicIssuesManager.GetPublicIssuesAsync(ownerName, reposName));
        }
    }
}
