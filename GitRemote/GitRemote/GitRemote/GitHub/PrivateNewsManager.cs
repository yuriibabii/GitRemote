using GitRemote.Models;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GitRemote.GitHub
{
    public class PrivateNewsManager
    {
        private const string NAMESPACE = "http://www.w3.org/2005/Atom";

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

        public async Task<IEnumerable<PrivateNewsModel>> GetPrivateNews()
        {
            try
            {

                var gitHubPrivateFeedItems = await GetPrivateFeedItems();

                var gitRemotePrivateFeedItems = new List<PrivateNewsModel>();

                foreach ( var item in gitHubPrivateFeedItems )
                {
                    var NewsItem = new PrivateNewsModel();
                    {


                    }
                    ;

                    gitRemotePrivateFeedItems.Add(NewsItem);
                }
                return new List<PrivateNewsModel>();

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

        private async Task<IEnumerable<PrivateNewsModel>> GetPrivateFeedItems()
        {
            var client = new HttpClient();

            var content = await client.GetStringAsync(_session.GetPrivateFeedUrl());

            if ( string.IsNullOrEmpty(content) ) return new List<PrivateNewsModel>();

            var result = XElement.Parse(content);

            var feeds = from entry in result.Elements("{" + NAMESPACE + "}entry") select entry;

            foreach ( var xElement in feeds )
            {
                var element = xElement.Element(XName.Get("title", NAMESPACE));

                if ( element != null )
                {
                    var title = element.Value;
                }
            }








//    ////var content = await new HttpClient().GetStringAsync(_session.GetPrivateFeedUrl());
//    //var feedDoc = XDocument.Load(_session.GetPrivateFeedUrl());

            //    //var root = feedDoc.Root;

            //    //var feeds = root?.Elements().Where<XElement>(( Func<XElement, bool> )( i => i.Name.LocalName.Equals("entry") )).Select<XElement, PrivateNewsModel>(( Func<XElement, PrivateNewsModel> )( item => new PrivateNewsModel()
            //    //{
            //    //    Description = Find(item, "summary").Value,
            //    //    Link = Find(item, "link").Attribute(( XName )"href").Value,
            //    //    Title = Find(item, "title").Value
            //    //} ));

            //    //return feeds;


            //    //var fp = new FeedParser();
            //    //var items = fp.Parse(_session.GetPrivateFeedUrl(), FeedType.Atom);
            //    //return items;


            //    //using ( HttpClient httpClient = new HttpClient() )
            //    //{
            //    //    string content = await httpClient.GetStringAsync(url).ConfigureAwait(false);
            //    //    objs = this.Parse(content, feedType);
            //    //}
            //}

        private static readonly
            Func<XElement, string, XElement> Find = ( Func<XElement, string, XElement> )( (item, name) => item.Elements().First<XElement>(( Func<XElement, bool> )( i => i.Name.LocalName.Equals(name) )) );

    }
}
