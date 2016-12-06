using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GitRemote.GitHub.Managers
{
    public class PullRequestsManager
    {
        private readonly GitHubClient _gitHubClient;
        private readonly string _ownerName;
        private readonly string _reposName;

        public PullRequestsManager(Session session, string ownerName, string reposName)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));
            _ownerName = ownerName;
            _reposName = reposName;
        }

        public async Task<IEnumerable<PullRequestModel>> GetPullRequestsAsync()
        {
            try
            {
                var request = new PullRequestRequest()
                {
                    State = ItemStateFilter.Open,
                    SortDirection = SortDirection.Descending
                };

                var options = new ApiOptions { PageCount = 1, PageSize = 30 };

                var gitHubPullRequests = await _gitHubClient.PullRequest.GetAllForRepository(_ownerName, _reposName, request, options);

                var gitRemotePullRequests = new List<PullRequestModel>();

                foreach ( var pullRequest in gitHubPullRequests )
                {
                    var model = new PullRequestModel
                    {
                        IsCommented = pullRequest.Comments > 0,
                        CommentsCount = pullRequest.Comments,
                        Title = pullRequest.Title,
                        OwnerName = StringService.CheckForNullOrEmpty(pullRequest.User.Name)
                            ? pullRequest.User.Name
                            : pullRequest.User.Login,
                        ImageUrl = pullRequest.User.AvatarUrl,
                        CreatedTime = TimeService.ConvertToFriendly(Convert.ToString(pullRequest.CreatedAt)),
                        Nomer = Convert.ToString(pullRequest.Number),
                    };

                    gitRemotePullRequests.Add(model);
                }

                return gitRemotePullRequests;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting pullRequests from github failed! " + ex.Message);
            }
        }
    }
}
