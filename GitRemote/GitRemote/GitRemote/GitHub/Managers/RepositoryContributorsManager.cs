using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.String;

namespace GitRemote.GitHub.Managers
{
    public class RepositoryContributorsManager
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _ownerName = Empty;
        private readonly string _reposName = Empty;

        public RepositoryContributorsManager(Session session, string ownerName, string reposName)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));

            _ownerName = ownerName;
            _reposName = reposName;
        }

        public async Task<List<RepositoryContributorModel>> GetRepositoryContributors()
        {
            var gitHubContributors = await _gitHubClient.Repository.GetAllContributors(_ownerName, _reposName);
            var gitRemoteContributors = new List<RepositoryContributorModel>();

            foreach ( var contributor in gitHubContributors )
            {
                var model = new RepositoryContributorModel
                {
                    Login = contributor.Login,
                    AvatarUrl = contributor.AvatarUrl,
                    CommitsCount = contributor.Contributions
                };

                gitRemoteContributors.Add(model);
            }

            return gitRemoteContributors;
        }
    }
}
