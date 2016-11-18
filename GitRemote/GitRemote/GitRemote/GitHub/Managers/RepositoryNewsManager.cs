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
            var payload = item["payload"];
            switch ( model.EventType )
            {
                case "PushEvent":
                    model.Target = payload["ref"].ToString().Split('/')[2];
                    model.CommitsCount = payload["size"].ToString();
                    model.Comment = payload["commits"][0]["message"].ToString();
                    model.ShaCode = payload["commits"][0]["sha"].ToString().Substring(0, 7);
                    model.ActionType = "pushed";
                    model.ActionTypeFontIcon = Commit;
                    model.IsBody = true;
                    model.IsSubtitle = true;
                    break;

                case "PullRequestEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = payload["number"].ToString();
                    if ( model.ActionType != "reopened" )
                    {
                        model.IsBody = true;
                        model.Body = payload["pull_request"]["title"].ToString();
                    }
                    model.ActionTypeFontIcon = PullRequest;
                    break;

                case "IssuesEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = payload["issue"]["number"].ToString();
                    model.Body = payload["issue"]["title"].ToString();
                    model.ActionTypeFontIcon = model.ActionType == "opened" ? IssueOpened
                                               : ( model.ActionType == "reopened" ? IssueReopened
                                               : IssueClosed );
                    model.IsBody = true;
                    break;

                case "IssueCommentEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = payload["issue"]["number"].ToString();
                    model.Body = payload["issue"]["title"].ToString();
                    model.ActionTypeFontIcon = Discussion;
                    model.IsBody = true;
                    break;

                case "CommitCommentEvent":
                    model.ActionType = "commented";
                    model.ShaCode = payload["comment"]["commit_id"].ToString().Substring(0, 10);
                    model.Body = payload["comment"]["body"].ToString();
                    model.ActionTypeFontIcon = Comment;
                    model.IsBody = true;
                    model.IsSubtitle = true;
                    break;

                case "CreateEvent":
                    model.ActionType = payload["ref_type"].ToString();
                    if ( model.ActionType == "tag" )
                        model.Nomer = payload["ref"].ToString();
                    else
                        model.Target = payload["ref"].ToString();
                    model.ActionTypeFontIcon = model.ActionType == "branch" ? Branch
                                              : ( model.ActionType == "repository" ? Repo
                                              : Tag );
                    break;

                case "DeleteEvent":
                    model.ActionType = payload["ref_type"].ToString();
                    if ( model.ActionType == "tag" )
                        model.Nomer = payload["ref"].ToString();
                    else
                        model.Target = payload["ref"].ToString();
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
                    model.ActionType = "repo_public";
                    model.ActionTypeFontIcon = Repo;
                    break;

                case "ReleaseEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Target = payload["release"]["name"].ToString();
                    model.ActionTypeFontIcon = CloudDownload;
                    break;

                case "WatchEvent":
                    model.ActionType = payload["action"].ToString();
                    model.ActionTypeFontIcon = Star;
                    break;

                default:
                    return false;
            }

            return true;
        }
    }
}
