using GitRemote.GitHub;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels
{
    public class DetailPageViewModel : BindableBase, INavigationAware
    {
        private Session _session;
        private GitHubClient _gitHubClient;

        public DetailPageViewModel()
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey("Session") ) return;

            _session = parameters["Session"] as Session;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(_session?.GetToken())));
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

    }
}
