using GitRemote.Models;
using GitRemote.Services;
using ModernHttpClient;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitRemote.GitHub.Managers
{
    public class PrivateNewsManager
    {
        private readonly Session _session;

        public PrivateNewsManager(Session session)
        {
            _session = session;
        }

        public PrivateNewsManager()
        { }

        /// <summary>
        /// Gets Private Url from GitHub API
        /// </summary>
        /// <param name="gitHubClient">Takes GitHub client with base auth credentials</param>
        /// <returns>Private Url</returns>
        public async Task<string> GetPrivateFeedUrlFromApiAsync(GitHubClient gitHubClient)
        {
            var task = await gitHubClient.Activity.Feeds.GetFeeds();

            return task.CurrentUserUrl;
        }

        /// <summary>
        /// Gets Private News via GitHub API, only 30 last items.
        /// </summary>
        /// <returns>IEnumerable of PrivateNewsModel</returns>
        public async Task<IEnumerable<PrivateNewsModel>> GetPrivateNews()
        {
            try
            {
                var gitHubPrivateFeedItems = await GetPrivateFeedItems();

                var gitRemotePrivateFeedItems = new List<PrivateNewsModel>();

                foreach ( var item in gitHubPrivateFeedItems )
                {
                    var newsItem = new PrivateNewsModel
                    {
                        Title = item?.Element(XName.Get("title", ConstantsService.AtomNamespace))?.Value,
                        Published = TimeService.ConvertToFriendly(
                                item?.Element(XName.Get("published", ConstantsService.AtomNamespace))?.Value),
                        ImageUrl = item?.Elements().ElementAtOrDefault(6).Attribute("url").Value // Hardcoded, but happy cuz works

                    };

                    var splitedTitle = newsItem.Title?.Split(' ');

                    if ( splitedTitle != null )
                    {
                        newsItem.Perfomer = splitedTitle[0];
                        newsItem.ActionType = splitedTitle[1];
                        newsItem.AdditionalTarget = newsItem.ActionType == "added"
                            ? splitedTitle[2]
                            : string.Empty;
                        newsItem.Target = newsItem.ActionType == "forked"
                            ? splitedTitle[2]
                            : splitedTitle[splitedTitle.Length - 1];

                        switch ( newsItem.ActionType )
                        {
                            case "added":
                                newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Person;
                                break;
                            case "created":
                                newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Repo;
                                break;
                            case "forked":
                                newsItem.ActionTypeFontIcon = FontIconsService.Octicons.RepoForked;
                                break;
                            case "starred":
                                newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Star;
                                break;
                            case "opened":
                                newsItem.ActionTypeFontIcon = FontIconsService.Octicons.OpenedIssue;
                                break;
                        }
                    }
                    gitRemotePrivateFeedItems.Add(newsItem);
                }
                return gitRemotePrivateFeedItems;
            }

            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting repos from github failed! " + ex.Message);
            }
        }


        /// <summary>
        /// Executes http request to private feed url, takes feed in Atom XML format
        /// </summary>
        /// <returns>Items(entries)</returns>
        private async Task<IEnumerable<XElement>> GetPrivateFeedItems()
        {
            var client = new HttpClient(new NativeMessageHandler());

            var feed = await client.GetStringAsync(_session.GetPrivateFeedUrl());

            if ( string.IsNullOrEmpty(feed) ) return new List<XElement>();

            var parsedFeed = XElement.Parse(feed);

            var entries = from entry
                          in parsedFeed.Elements("{" + ConstantsService.AtomNamespace + "}entry")
                          select entry;

            return entries;
        }


    }
}
