using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GitRemote.GitHub
{
    public class StarredRepositoriesManager
    {
        private readonly GitHubClient _gitHubClient;

        public StarredRepositoriesManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<IEnumerable<StarredRepositoryModel>> GetStarsAsync()
        {
            try
            {
                var gitHubStarredRepos = await _gitHubClient.Activity.Starring.GetAllForCurrent();

                var gitRemoteStarredRepos = new List<StarredRepositoryModel>();

                foreach ( var starredRepos in gitHubStarredRepos )
                {
                    var starredReposModel = new StarredRepositoryModel
                    {
                        StarredRepositoryTypeIcon = GetStarredRepositoryTypeIcon(starredRepos),
                        StarredRepositoryDescription = starredRepos.Description,
                        IsDescription = !string.IsNullOrEmpty(starredRepos.Description),
                        StarredRepositoryLanguage = starredRepos.Language,
                        StarredRepositoryStarIcon = FontIconsService.Octicons.Star,
                        StarredRepositoryStarsCount = Convert.ToString(starredRepos.StargazersCount),
                        StarredRepositoryForkIcon = FontIconsService.Octicons.RepoForked,
                        StarredRepositoryForksCount = Convert.ToString(starredRepos.ForksCount)
                    };

                    //Separating Full name to have repos name bold
                    var fullName = starredRepos.FullName;
                    var separateIndex = fullName.LastIndexOf('/') + 1;
                    starredReposModel.StarredRepositoryName = fullName.Substring(separateIndex);
                    starredReposModel.StarredRepositoryPath = fullName.Substring(0, separateIndex);

                    gitRemoteStarredRepos.Add(starredReposModel);
                }

                return gitRemoteStarredRepos;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting starredRepos from github failed! " + ex.Message);
            }
        }

        /// <summary>
        /// Decides what is starredRepos type and return Icon for it
        /// </summary>
        /// <param name="repos">starredRepos</param>
        /// <returns>Octicon FontIcon code</returns>
        private string GetStarredRepositoryTypeIcon(Repository repos)
        {
            return repos.Fork ? FontIconsService.Octicons.RepoForked
                              : ( repos.Private ? FontIconsService.Octicons.Lock
                                                : FontIconsService.Octicons.Repo );

        }
    }
}
