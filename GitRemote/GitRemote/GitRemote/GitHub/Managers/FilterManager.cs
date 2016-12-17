using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitRemote.GitHub.Managers
{
    public class FilterManager
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _ownerName;
        private readonly string _reposName;

        public string AssignedName { get; set; } = string.Empty;

        public FilterManager(Session session, string ownerName, string reposName)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));

            _ownerName = ownerName;
            _reposName = reposName;
        }

        public async Task<IReadOnlyList<User>> GetAssigneesAsync()
        {
            return await _gitHubClient.Repository.Collaborator.GetAll(_ownerName, _reposName);
        }
    }
}
