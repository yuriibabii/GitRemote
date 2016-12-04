using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.GitHub.Managers
{
    public class PublicIssuesManager
    {
        private readonly GitHubClient _gitHubClient;

        public PublicIssuesManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                   new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<IEnumerable<IssueModel>> GetPublicIssuesAsync(string ownerName, string reposName)
        {
            try
            {
                var request = new RepositoryIssueRequest
                {
                    State = ItemStateFilter.Open,
                    Filter = IssueFilter.All,
                    SortDirection = SortDirection.Descending
                };

                var options = new ApiOptions { PageCount = 1, PageSize = 30 };

                var gitHubPublicIssues = await _gitHubClient.Issue.GetAllForRepository(ownerName, reposName, request, options);

                var gitRemotePublicIssues = new List<IssueModel>();

                foreach ( var issue in gitHubPublicIssues )
                {
                    var issueModel = new IssueModel
                    {
                        IsCommented = issue.Comments > 0,
                        CommentsCount = issue.Comments,
                        Title = issue.Title,
                        OwnerName = StringService.CheckForNullOrEmpty(issue.User.Name)
                            ? issue.User.Name
                            : issue.User.Login,
                        ImageUrl = issue.User.AvatarUrl,
                        CreatedTime = TimeService.ConvertToFriendly(Convert.ToString(issue.CreatedAt)),
                        Nomer = Convert.ToString(issue.Number),
                        IsPullRequest = issue.PullRequest != null
                    };

                    CopyColors(issueModel.Labels, issue.Labels.Select(l => l.Color));

                    gitRemotePublicIssues.Add(issueModel);
                }

                return gitRemotePublicIssues;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting issues from github failed! " + ex.Message);
            }
        }

        public void CopyColors(ObservableCollection<Color> colors, IEnumerable<string> strings)
        {
            var i = 0;
            foreach ( var s in strings )
            {
                colors[i] = Color.FromHex(s);
                ++i;
            }

        }
    }
}
