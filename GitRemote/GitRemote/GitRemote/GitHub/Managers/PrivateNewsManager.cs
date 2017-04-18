using GitRemote.Models;
using GitRemote.Services;
using ModernHttpClient;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using MvvmHelpers;
using static GitRemote.Models.PrivateNewsModel;
using static GitRemote.Services.ExceptionsService;
using static GitRemote.Services.FontIconsService;

namespace GitRemote.GitHub.Managers
{
    public class PrivateNewsManager
    {
        private readonly Session _session;

        public PrivateNewsManager(Session session)
        {
            _session = session;
        }

        public PrivateNewsManager() { }

        /// <summary>
        /// Gets Private Url from GitHub API
        /// </summary>
        /// <param name="gitHubClient">Takes GitHub client with base auth credentials</param>
        /// <returns>Private Url</returns>
        public async Task<string> GetPrivateFeedUrlFromApiAsync(GitHubClient gitHubClient)
        {
            var feeds = await gitHubClient.Activity.Feeds.GetFeeds();
            return feeds.CurrentUserUrl;
        }

        /// <summary>
        /// Gets Private News via GitHub API, only 30 last items.
        /// </summary>
        /// <returns>IEnumerable of PrivateNewsModel</returns>
        public async Task<ObservableRangeCollection<PrivateNewsModel>> GetPrivateNews(int pageNumber = 1)
        {
            try
            {
                var gitHubPrivateFeedItems = await GetPrivateFeedItems(pageNumber);
                var gitRemotePrivateFeedItems = new List<PrivateNewsModel>();

                foreach (var item in gitHubPrivateFeedItems)
                {
                    var newsItem = new PrivateNewsModel();
                    var title = XName.Get("title", ConstantsService.AtomNamespace);
                    newsItem.Title = item.Element(title)?.Value;
                    var date = XName.Get("published", ConstantsService.AtomNamespace);
                    newsItem.Date = TimeService.ConvertToFriendly(item.Element(date)?.Value);
                    newsItem.AvatarUrl = item.Elements().ElementAtOrDefault(6).Attribute("url").Value; // Hardcoded, but happy cuz works

                    var splitedTitle = newsItem.Title?.Split(' ');

                    if (splitedTitle != null)
                    {
                        newsItem.Perfomer = splitedTitle[0];
                        newsItem.ActionType = GetActionType(splitedTitle[1]);
                        newsItem.AdditionalTarget = newsItem.ActionType == ActionTypes.Added
                            ? splitedTitle[2]
                            : string.Empty;

                        if (newsItem.ActionType == ActionTypes.Forked ||
                            newsItem.ActionType == ActionTypes.Made)
                        {
                            newsItem.Target = splitedTitle[2];
                        }
                        else
                        {
                            newsItem.Target = splitedTitle[splitedTitle.Length - 1];
                        }

                        switch (newsItem.ActionType)
                        {
                            case ActionTypes.Added:
                                newsItem.ActionTypeFontIcon = Octicons.Person;
                                break;
                            case ActionTypes.Created:
                                newsItem.ActionTypeFontIcon = Octicons.Repo;
                                break;
                            case ActionTypes.Forked:
                                newsItem.ActionTypeFontIcon = Octicons.RepoForked;
                                break;
                            case ActionTypes.Starred:
                                newsItem.ActionTypeFontIcon = Octicons.Star;
                                break;
                            case ActionTypes.Opened:
                                newsItem.ActionTypeFontIcon = Octicons.IssueOpened;
                                break;
                            case ActionTypes.Made:
                                newsItem.ActionTypeFontIcon = Octicons.Repo;
                                break;
                            default: throw new ActionTypeNotFoundException();
                        }
                    }
                    gitRemotePrivateFeedItems.Add(newsItem);
                }

                return new ObservableRangeCollection<PrivateNewsModel>(gitRemotePrivateFeedItems);
            }

            catch (WebException exn)
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + exn.Message);
            }
            catch (Exception exception)
            {
                throw new Exception("Getting private news from github failed! " + exception.Message);
            }
        }

        /// <summary>
        /// Executes http request to private feed url, takes feed in Atom XML format
        /// </summary>
        /// <returns>Items(entries)</returns>
        private async Task<IEnumerable<XElement>> GetPrivateFeedItems(int pageNumber)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var feed = await client.GetStringAsync(_session.GetPrivateFeedUrl() + $"&page={pageNumber}");

                if (string.IsNullOrEmpty(feed)) return new List<XElement>();

                var parsedFeed = XElement.Parse(feed);

                var entries = from entry
                              in parsedFeed.Elements("{" + ConstantsService.AtomNamespace + "}entry")
                              select entry;

                return entries;
            }
        }

        private ActionTypes GetActionType(string hardCodedType)
        {
            switch (hardCodedType)
            {
                case "added": return ActionTypes.Added;
                case "created": return ActionTypes.Created;
                case "forked": return ActionTypes.Forked;
                case "starred": return ActionTypes.Starred;
                case "opened": return ActionTypes.Opened;
                case "made": return ActionTypes.Made;
                default: throw new ActionTypeNotFoundException();
            }
        }
    }
}
