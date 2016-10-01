using GitRemote.GitHub;
using GitRemote.Services;
using Octokit;
using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels
{
    public class ProfilePageViewModel : BindableBase, INavigationAware
    {
        private readonly GitHubClient _gitHubClient;
        private readonly GitRemoteClient _gitRemoteClient;

        public ProfilePageViewModel()
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName));
            _gitRemoteClient = new GitRemoteClient();
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( parameters.ContainsKey("Token") )
                _gitHubClient.Credentials = new Credentials(parameters["Token"].ToString());
            if ( parameters.ContainsKey("Login") )
                _gitRemoteClient.Login = parameters["Login"].ToString();
        }
    }
}
