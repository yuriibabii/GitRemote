using GitRemote.DI;
using GitRemote.Models;
using GitRemote.Services;
using Newtonsoft.Json.Linq;
using Octokit;
using Octokit.Internal;
using Plugin.Share;
using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
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
        private readonly Session _session;
        private readonly string _ownerName;
        private readonly string _reposName;
        private readonly GitHubClient _gitHubClient;

        public RepositoryNewsManager(Session session, string ownerName, string reposName)
        {
            _session = session;
            _ownerName = ownerName;
            _reposName = reposName;
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));
        }

        public async Task<IEnumerable<RepositoryNewsModel>> GetRepositoryNews()
        {
            try
            {
                var gitHubReposNewsItems = await GetRepositoryNewsItems(_ownerName, _reposName);

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
            var client = new RestClient(ConstantsService.GitHubApiLink)
            {
                Authenticator = new HttpBasicAuthenticator
                    (new NetworkCredential(_session.Login, _session.GetToken()), AuthHeader.Www)
            };

            using ( client )
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
                    if ( Convert.ToInt32(model.CommitsCount.Split(' ')[0]) > 0 )
                    {
                        model.Comment = payload["commits"][0]["message"].ToString();
                        model.ShaCode = payload["commits"][0]["sha"].ToString().Substring(0, 7);
                        model.IsBody = true;
                        model.IsSubtitle = true;
                    }
                    model.ActionType = "pushed";
                    model.ActionTypeFontIcon = FontIconsService.Octicons.Commit;
                    break;

                case "PullRequestEvent":
                    model.ActionType = payload["action"].ToString();
                    model.Nomer = payload["number"].ToString();
                    if ( model.ActionType != "reopened" )
                    {
                        model.IsBody = true;
                        model.Body = payload["pull_request"]["title"].ToString();
                    }
                    model.ActionTypeFontIcon = FontIconsService.Octicons.PullRequest;
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
                    model.ActionTypeFontIcon = model.ActionType == "branch" ? FontIconsService.Octicons.Branch
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

        public async Task<bool> CheckStar()
        {
            return await _gitHubClient.Activity.Starring.CheckStarred(_ownerName, _reposName);
        }

        public async Task StarRepository()
        {
            await _gitHubClient.Activity.Starring.StarRepo(_ownerName, _reposName);
        }

        public async Task UnstarRepository()
        {
            await _gitHubClient.Activity.Starring.RemoveStarFromRepo(_ownerName, _reposName);
        }

        public async Task ForkRepository()
        {
            await _gitHubClient.Repository.Forks.Create(_ownerName, _reposName, new NewRepositoryFork());
        }

        public async Task OpenInBrowser(IDevice device)
        {
            await device.LaunchUriAsync(new Uri($"{ConstantsService.GitHubOfficialPageUrl}{_ownerName}/{_reposName}"));
        }

        public async Task ShareLinkOnRepository()
        {
            await CrossShare.Current.ShareLink($"{ConstantsService.GitHubOfficialPageUrl}{_ownerName}/{_reposName}");
        }
    }
}
