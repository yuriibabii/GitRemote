using GitRemote.Models;
using GitRemote.Services;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using static GitRemote.Services.FontIconsService.Octicons;

namespace GitRemote.GitHub.Managers
{
    public class RepositoryNewsManager
    {
        public async Task<IEnumerable<RepositoryNewsModel>> GetRepositoryNews(string login, string reposName)
        {
            try
            {
                var gitHubReposNewsItems = await GetRepositoryNewsItems(login, reposName);

                var gitRemoteNewsItems = new List<RepositoryNewsModel>();

                foreach ( var item in gitHubReposNewsItems )
                {
                    var model = new RepositoryNewsModel
                    {
                        AvatarImageUrl = item["actor"]["avatar_url"].ToString(),
                        EventType = item["type"].ToString(),
                        PublishedTime = TimeService.ConvertToFriendly(item["created_at"].ToString()),
                        Perfomer = item["actor"]["display_login"].ToString()
                    };

                    if ( SetSpecificProperties(model, item) )
                        gitRemoteNewsItems.Add(model);

                }

                return gitRemoteNewsItems;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting reposNews from github failed! " + ex.Message);
            }
        }

        private async Task<JArray> GetRepositoryNewsItems(string login, string reposName)
        {
            using ( var client = new RestClient(ConstantsService.GitHubApiLink) )
            {
                var request = new RestRequest($"/repos/{login}/{reposName}/events", Method.GET)
                {
                    Serializer = { ContentType = "application/json" }
                };

                var responceResult = await client.Execute(request);
                var arrayOfElements = JArray.Parse(responceResult.Content);

                return arrayOfElements;
            }
        }

        private bool SetSpecificProperties(RepositoryNewsModel model, JToken item)
        {
            JArray commits;
            var payload = item["payload"];
            switch ( model.EventType )
            {

                case "PushEvent":
                    model.Target = payload["ref"].ToString().Split('/')[2];
                    commits = JArray.Parse(payload["commits"].ToString());
                    model.CommitsCount = commits.Count;
                    model.IsCommits = true;
                    model.Comment = commits[0]["message"].ToString();
                    model.PayloadHead = payload["head"].ToString().Substring(0, 7);
                    model.IsPayloadHead = true;
                    model.ActionType = "pushed";
                    model.ActionTypeFontIcon = Commit;
                    break;

                case "PullRequestEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = Convert.ToInt32(payload["number"]);
                    if ( model.ActionType != "reopened" )
                    {
                        model.IsTitle = true;
                        model.Title = payload["title"].ToString();
                    }
                    model.ActionTypeFontIcon = PullRequest;
                    break;

                case "IssuesEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = Convert.ToInt32(payload["issue"]["number"]);
                    model.Title = payload["issue"]["title"].ToString();
                    model.ActionTypeFontIcon = model.ActionType == "opened" ? IssueOpened
                                               : ( model.ActionType == "reopened" ? IssueReopened
                                               : IssueClosed );
                    break;

                case "IssueCommentEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = Convert.ToInt32(payload["issue"]["number"]);
                    model.Title = payload["issue"]["title"].ToString();
                    model.ActionTypeFontIcon = Discussion;
                    break;

                case "CommitCommentEvent":
                    model.ActionType = "commented";
                    model.PayloadHead = payload["commit_id"].ToString().Substring(0, 10);
                    model.Title = payload["comment"]["body"].ToString();
                    model.ActionTypeFontIcon = Comment;
                    break;

                case "CreateEvent":
                    model.ActionType = payload["ref_type"].ToString();
                    if ( model.ActionType == "tag" )
                        model.Nomer = Convert.ToInt32(payload["ref"]);
                    model.ActionTypeFontIcon = model.ActionType == "branch" ? Branch
                                              : ( model.ActionType == "repository" ? Repo
                                              : Tag );
                    break;

                case "DeleteEvent":
                    model.ActionType = payload["ref_type"].ToString();
                    if ( model.ActionType == "tag" )
                        model.Nomer = Convert.ToInt32(payload["ref"]);
                    else
                        model.Target = ( payload["ref"] ).ToString();
                    model.ActionTypeFontIcon = Trashcan;
                    break;

                case "GollumEvent":
                    model.ActionType = "wiki_updated";
                    model.ActionTypeFontIcon = Book;
                    break;

                case "MemberEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Target = payload["member"]["login"].ToString();
                    model.ActionTypeFontIcon = Person;
                    break;

                case "PublicEvent":

                    break;

                case "ReleaseEvent":

                    break;

                case "RepositoryEvent":

                    break;

                case "WatchEvent":

                    break;

                default:
                    return false;
            }

            return true;
        }
    }
}
