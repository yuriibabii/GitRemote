using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GitRemote.DI;
using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;

namespace GitRemote.GitHub.Managers
{
    public class GistsManager
    {
        public static string Tab = "Mine";
        private static GitHubClient _gitHubClient;
        public static bool IsGitHubClient => _gitHubClient != null;
        private static string _minePageTitle = string.Empty;
        private static string _starredPageTitle = string.Empty;
        private static string _allPageTitle = string.Empty;
        private const string NoAvatarImage = "ic_list_NoAvatar.png";

        public static void SetGitHubClient(Session session)
        {
            if ( _gitHubClient == null )
                _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                    new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<IEnumerable<GistModel>> GetGistsAsync()
        {
            try
            {
                IReadOnlyList<Gist> gitHubGists;

                switch ( Tab )
                {
                    case "Mine":
                        Tab = "Starred";
                        gitHubGists = await _gitHubClient.Gist.GetAll();
                        break;
                    case "Starred":
                        Tab = "All";
                        gitHubGists = await _gitHubClient.Gist.GetAllStarred();
                        break;
                    case "All":
                        Tab = "Mine";
                        var options = new ApiOptions { PageCount = 1, PageSize = 50 };
                        gitHubGists = await _gitHubClient.Gist.GetAllPublic(options);
                        break;
                    default:
                        gitHubGists = new Gist[0];
                        break;
                }

                var gitRemoteGists = new List<GistModel>();

                foreach ( var gist in gitHubGists )
                {
                    var gistModel = new GistModel
                    {
                        Id = gist?.Id,
                        CommentsCount = gist.Comments,
                        IsCommented = gist.Comments > 0,
                        CreatedTime = TimeService.ConvertToFriendly(Convert.ToString(gist.CreatedAt)),
                        FilesCount = gist.Files.Count,
                        ImageUrl = gist.Owner?.AvatarUrl ?? NoAvatarImage
                    };

                    if ( gist.Owner == null )
                        gistModel.OwnerName = "Anonymous";
                    else
                        gistModel.OwnerName = StringService.CheckForNullOrEmpty(gist.Owner?.Name)
                            ? gist.Owner?.Name
                            : gist.Owner?.Login;

                    gistModel.Description = StringService.CheckForNullOrEmpty(gist.Description)
                        ? gist.Description
                        : "No description given.";

                    gitRemoteGists.Add(gistModel);
                }

                return gitRemoteGists;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting gists from github failed! " + ex.Message);
            }
        }

        public static void SetTabsTitles(IMetricsHelper metricsHelper)
        {
            _minePageTitle = string.Empty;
            _starredPageTitle = string.Empty;
            _allPageTitle = string.Empty;

            var spaceWidth = metricsHelper.GetWidthOfString("i i") - metricsHelper.GetWidthOfString("ii");
            var mineTitleWidth = metricsHelper.GetWidthOfString("MINE") + 2 * spaceWidth;
            var starredTitleWidth = metricsHelper.GetWidthOfString("STARRED");
            var allTitleWidth = metricsHelper.GetWidthOfString("ALL") + 2 * spaceWidth;
            var restOfSpace = App.ScreenWidth - ( mineTitleWidth + starredTitleWidth + allTitleWidth ) - 7;
            var minePageTabWidth = restOfSpace / 2;
            var allPageTabWidth = restOfSpace - minePageTabWidth;
            var amountOfMineTabSpaces = minePageTabWidth / spaceWidth;
            amountOfMineTabSpaces = amountOfMineTabSpaces - amountOfMineTabSpaces / 3;
            var amountOfAllTabSpaces = allPageTabWidth / spaceWidth;
            amountOfAllTabSpaces = amountOfAllTabSpaces - amountOfAllTabSpaces / 3;
            _minePageTitle = _minePageTitle.PadLeft(amountOfMineTabSpaces - 2, ' ') + "MINE" + "  ";
            _starredPageTitle = "STARRED";
            _allPageTitle = "  " + "ALL" + _allPageTitle.PadRight(amountOfAllTabSpaces - 2, ' ');
        }

        public string GetTabTitle()
        {
            return Tab == "Mine"
                ? _minePageTitle
                : ( Tab == "Starred"
                        ? _starredPageTitle
                        : _allPageTitle );
        }
    }

}

