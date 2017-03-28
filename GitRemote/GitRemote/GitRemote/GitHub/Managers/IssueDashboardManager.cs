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
    public class IssueDashboardManager
    {
        public static string Tab = "Watched";
        private static GitHubClient _gitHubClient;
        public static bool IsGitHubClient => _gitHubClient != null;

        public static void SetGitHubClient(Session session)
        {
            if ( _gitHubClient == null )
                _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                    new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<IEnumerable<IssueModel>> GetIssuesAsync()
        {
            try
            {
                IReadOnlyList<Issue> gitHubIssues;
                var request = new IssueRequest { State = ItemStateFilter.Open };
                switch ( Tab )
                {
                    case "Watched":
                        Tab = "Assigned";
                        request.Filter = IssueFilter.Subscribed;
                        gitHubIssues = await _gitHubClient.Issue.GetAllForCurrent(request);
                        break;
                    case "Assigned":
                        Tab = "Created";
                        request.Filter = IssueFilter.Assigned;
                        gitHubIssues = await _gitHubClient.Issue.GetAllForCurrent(request);
                        break;
                    case "Created":
                        Tab = "Mentioned";
                        request.Filter = IssueFilter.Created;
                        gitHubIssues = await _gitHubClient.Issue.GetAllForCurrent(request);
                        break;
                    case "Mentioned":
                        Tab = "Watched";
                        request.Filter = IssueFilter.Mentioned;
                        gitHubIssues = await _gitHubClient.Issue.GetAllForCurrent(request);
                        break;
                    default:
                        gitHubIssues = new Issue[0];
                        break;
                }

                var gitRemoteIssues = new List<IssueModel>();

                foreach ( var issue in gitHubIssues )
                {
                    var issueModel = new IssueModel
                    {
                        IsCommented = issue.Comments > 0,
                        CommentsCount = issue.Comments,
                        Title = issue.Title,
                        Repository = issue.Repository.FullName,
                        OwnerName = StringService.IsNullOrEmpty(issue.User.Name)
                            ? issue.User.Login
                            : issue.User.Name,
                        ImageUrl = issue.User.AvatarUrl,
                        CreatedTime = TimeService.ConvertToFriendly(Convert.ToString(issue.CreatedAt)),
                        Nomer = Convert.ToString(issue.Number),
                        IsPullRequest = issue.PullRequest != null
                    };

                    CopyColors(issueModel.Labels, issue.Labels.Select(l => l.Color));

                    gitRemoteIssues.Add(issueModel);
                }

                return gitRemoteIssues;
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
