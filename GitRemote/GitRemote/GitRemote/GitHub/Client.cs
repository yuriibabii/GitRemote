using GitRemote.Services;
using Octokit;
using Octokit.Internal;

namespace GitRemote.GitHub
{
    public class Client
    {
        private readonly GitHubClient _client;

        public Client(string login, string password)
        {
            _client = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(login, password)));
        }

        public GitHubClient GetClient()
        {
            return _client;
        }
    }
}
