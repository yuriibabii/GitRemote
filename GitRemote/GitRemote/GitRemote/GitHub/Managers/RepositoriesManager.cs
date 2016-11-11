using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;

namespace GitRemote.GitHub.Managers
{
    public class RepositoriesManager
    {
        private readonly GitHubClient _gitHubClient;

        public RepositoriesManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        /// <summary>
        /// Sends request via API to Github acc and takes all repos from it, then converts it to GitRemote repos and groups it
        /// </summary>
        /// <returns>Grouped Obs Collection of GitRemote repos</returns>
        public async Task<ObservableCollection<GroupingModel<string, RepositoryModel>>> GetRepositoriesAsync()
        {
            try
            {
                var gitHubRepos = await _gitHubClient.Repository.GetAllForCurrent();

                var gitRemoteRepos = new List<RepositoryModel>();

                foreach ( var repository in gitHubRepos )
                {
                    var repos = new RepositoryModel
                    {
                        RepositoryTypeIcon = GetRepositoryTypeIcon(repository),
                        RepositoryName = repository.Name,
                        RepositoryDescription = repository.Description,
                        IsDescription = !string.IsNullOrEmpty(repository.Description),
                        RepositoryLanguage = repository.Language ?? " ",
                        RepositoryStarIcon = FontIconsService.Octicons.Star,
                        RepositoryStarsCount = Convert.ToString(repository.StargazersCount),
                        RepositoryForkIcon = FontIconsService.Octicons.RepoForked,
                        RepositoryForksCount = Convert.ToString(repository.ForksCount)
                    };

                    gitRemoteRepos.Add(repos);
                }

                var groupedGitRemoteRepos = from model in gitRemoteRepos //foreach rep
                                            orderby model.RepositoryName // sort by OwnerName
                                            group model by Convert.ToString(model.RepositoryName[0]).ToUpper() into modelGroup //Save each group and its key
                                            select new GroupingModel<string, RepositoryModel>(modelGroup.Key.ToUpper(), modelGroup); //Convert it to collection

                return new ObservableCollection<GroupingModel<string, RepositoryModel>>(groupedGitRemoteRepos);
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
        /// Decides what is repos type and return Icon for it
        /// </summary>
        /// <param name="repos">Repository</param>
        /// <returns>Octicon FontIcon code</returns>
        private string GetRepositoryTypeIcon(Repository repos)
        {
            return repos.Fork ? FontIconsService.Octicons.RepoForked
                              : ( repos.Private ? FontIconsService.Octicons.Lock
                                                : FontIconsService.Octicons.Repo );

        }

    }
}
