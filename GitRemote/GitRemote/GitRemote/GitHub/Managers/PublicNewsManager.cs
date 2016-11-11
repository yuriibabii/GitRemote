namespace GitRemote.GitHub.Managers
{
    public class PublicNewsManager
    {
        ///// <summary>
        ///// Gets Public News via GitHub API, only 30 last items.
        ///// </summary>
        ///// <returns>IEnumerable of PublicNewsModel</returns>
        //public async Task<IEnumerable<PublicNewsModel>> GetPublicNews(string login)
        //{
        //    try
        //    {
        //        var feedclient = new FeedsClient(new ApiConnection
        //            (new Connection(new ProductHeaderValue(ConstantsService.AppName))));
        //        var feed = await feedclient.GetFeeds();
        //        var gitHubPublicFeedItems = await GetPublicFeedItems();

        //        var gitRemotePublicFeedItems = new List<PublicNewsModel>();

        //        foreach ( var item in gitHubPublicFeedItems )
        //        {
        //            var newsItem = new PublicNewsModel
        //            {
        //                //Title = item?.Element(XName.Get("title", ConstantsService.AtomNamespace))?.Value,
        //                //Published = TimeService.ConvertToFriendly(
        //                //        item?.Element(XName.Get("published", ConstantsService.AtomNamespace))?.Value),
        //                //ImageUrl = item?.Elements().ElementAtOrDefault(6).Attribute("url").Value // Hardcoded, but happy cuz works

        //            };

        //            var splitedTitle = newsItem.Title?.Split(' ');

        //            if ( splitedTitle != null )
        //            {
        //                newsItem.Perfomer = splitedTitle[0];
        //                newsItem.ActionType = splitedTitle[1];
        //                newsItem.AdditionalTarget = newsItem.ActionType == "added"
        //                    ? splitedTitle[2]
        //                    : string.Empty;
        //                newsItem.Target = newsItem.ActionType == "forked"
        //                    ? splitedTitle[2]
        //                    : splitedTitle[splitedTitle.Length - 1];

        //                switch ( newsItem.ActionType )
        //                {
        //                    case "added":
        //                        newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Person;
        //                        break;
        //                    case "created":
        //                        newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Repo;
        //                        break;
        //                    case "forked":
        //                        newsItem.ActionTypeFontIcon = FontIconsService.Octicons.RepoForked;
        //                        break;
        //                    case "starred":
        //                        newsItem.ActionTypeFontIcon = FontIconsService.Octicons.Star;
        //                        break;
        //                    case "opened":
        //                        newsItem.ActionTypeFontIcon = FontIconsService.Octicons.OpenedIssue;
        //                        break;
        //                }
        //            }
        //            gitRemotePublicFeedItems.Add(newsItem);
        //        }
        //        return gitRemotePublicFeedItems;
        //    }

        //    catch ( WebException ex )
        //    {
        //        throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
        //    }
        //    catch ( Exception ex )
        //    {
        //        throw new Exception("Getting repos from github failed! " + ex.Message);
        //    }
    }


    ///// <summary>
    ///// Executes http request to Public feed url, takes feed in Atom XML format
    ///// </summary>
    ///// <returns>Items(entries)</returns>
    //private async Task<IEnumerable<XElement>> GetPublicFeedItems()
    //{

    //    var client = new HttpClient(new NativeMessageHandler());

    //    var feed = await client.GetStringAsync();

    //    if ( string.IsNullOrEmpty(feed) ) return new List<XElement>();

    //    var parsedFeed = XElement.Parse(feed);

    //    var entries = from entry
    //                  in parsedFeed.Elements("{" + ConstantsService.AtomNamespace + "}entry")
    //                  select entry;

    //    return entries;
    //}


}


