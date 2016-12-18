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

        public string AssignedName { get; set; } = "Anyone";
        public string MilestoneName { get; set; } = "None";

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

        public async Task<IReadOnlyList<Milestone>> GetMilestonesAsync()
        {
            return await _gitHubClient.Issue.Milestone.GetAllForRepository
                (_ownerName, _reposName, new MilestoneRequest { State = ItemStateFilter.All });
        }
    }
}
