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

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey("Session") ) return;

            _session = parameters["Session"] as Session;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(_session?.GetToken())));
            
            //var user = await _gitHubClient.Activity.Feeds.GetFeeds();

            //var url = $"https://github.com/{_session?.Login}.private.atom?token={_session?.GetToken()}";
            //var request = WebRequest.Create(url);
            //using ( var responce = await request.GetResponseAsync() )
            //{
            //    var stream = responce.GetResponseStream();
            //    var streamReader = new StreamReader(stream);
            //    var line = streamReader.ReadLine();
            //    while ( line != null )
            //    {
            //        line = streamReader.ReadLine();
            //        Debug.WriteLine(line);
            //    }


            //}
        }
    }
}
