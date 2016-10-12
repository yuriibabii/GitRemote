using GitRemote.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace GitRemote.GitHub
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

        public async Task<string> GetPrivateFeedUrlFromApiAsync(GitHubClient gitHubClient)
        {
            var task = await gitHubClient.Activity.Feeds.GetFeeds();

            return task.CurrentUserUrl;
        }

        public ObservableCollection<PrivateNewsModel> GetPrivateNews()
        {
            try
            {
                var gitHubPrivateFeedItems = GetPrivateFeedItems();

                var gitRemotePrivateFeedItems = new List<PrivateNewsModel>();

                foreach ( var item in gitHubPrivateFeedItems )
                {
                    var NewsItem = new PrivateNewsModel();
                    {
                        var title = item.Title;

                    };

                    gitRemotePrivateFeedItems.Add(NewsItem);
                }

                return new ObservableCollection<PrivateNewsModel>(gitRemotePrivateFeedItems);
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

        private IEnumerable<SyndicationItem> GetPrivateFeedItems()
        {
            var formatter = new Atom10FeedFormatter();

            if ( _session == null )
                throw new Exception("Be careful with calling this method without session");

            using ( var reader = XmlReader.Create(_session.GetPrivateFeedUrl()) )
            {
                formatter.ReadFrom(reader);
            }

            return formatter.Feed.Items;
        }
    }
}
