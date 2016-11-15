using GitRemote.Models;
using GitRemote.Services;
using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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
                        PublishedTime = item["created_at"].ToString(),
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
            switch ( model.EventType )
            {
                case "PushEvent":
                    model.Target = item["payload"]["ref"].ToString().Split('/')[2];
                    model.CommitsCount = item[]
                    break;

                case "PullRequestEvent":

                    break;

                case "IssuesEvent":

                    break;

                case "IssueCommentEvent":

                    break;

                case "CommitCommentEvent":

                    break;

                case "CreateEvent":

                    break;

                case "DeleteEvent":

                    break;

                case "GollumEvent":

                    break;

                case "MemberEvent":

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
