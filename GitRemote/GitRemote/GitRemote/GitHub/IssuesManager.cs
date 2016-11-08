using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GitRemote.GitHub
{
    public class IssuesManager
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
                    //issue.Labels;
                    //issue.User.Login Name;
                    //issue.CreatedAt;
                    //issue.Title;
                    //issue.State;
                    //issue.Number;
                    //issue.Comments;
                    //issue.Repository;
                    //issue.

                    var issueModel = new IssueModel
                    {

                        //Id = gist?.Id,
                        //CommentsCount = gist.Comments,
                        //IsCommented = gist.Comments > 0,
                        //CreatedTime = TimeService.ConvertToFriendly(Convert.ToString(gist.CreatedAt)),
                        //FilesCount = gist.Files.Count,
                        //ImageUrl = gist.Owner?.AvatarUrl ?? NoAvatarImage
                    };

                    //if ( gist.Owner == null )
                    //    gistModel.OwnerName = "Anonymous";
                    //else
                    //    gistModel.OwnerName = StringService.CheckForNullOrEmpty(gist.Owner?.Name)
                    //        ? gist.Owner?.Name
                    //        : gist.Owner?.Login;

                    //gistModel.Description = StringService.CheckForNullOrEmpty(gist.Description)
                    //    ? gist.Description
                    //    : "No description given.";

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
    }
}
