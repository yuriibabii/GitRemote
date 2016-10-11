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
    public class RepositoriesManager
    {
        private readonly GitHubClient _gitHubClient;

        public RepositoriesManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<List<RepositoryModel>> GetRepositoriesAsync()
        {
            try
            {
                var reposRequest = new RepositoryRequest { Sort = RepositorySort.FullName };
                var gitHubRepos = await _gitHubClient.Repository.GetAllForCurrent(reposRequest);

                var gitRemoteRepos = new List<RepositoryModel>();

                foreach ( var repository in gitHubRepos )
                {
                    var repos = new RepositoryModel
                    {
                        RepositoryTypeIcon = GetRepositoryTypeIcon(repository),
                        RepositoryName = repository.Name,
                        RepositoryDescription = repository.Description,
                        IsDescription = !string.IsNullOrEmpty(repository.Description),
                        RepositoryLanguage = repository.Language,
                        RepositoryStarIcon = FontIconsService.Octicons.Star,
                        RepositoryStarsCount = Convert.ToString(repository.StargazersCount),
                        RepositoryForkIcon = FontIconsService.Octicons.RepoForked,
                        RepositoryForksCount = Convert.ToString(repository.ForksCount)
                    };

                    gitRemoteRepos.Add(repos);
                }

                return gitRemoteRepos;
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

        private string GetRepositoryTypeIcon(Repository repos)
        {
            return repos.Fork ? FontIconsService.Octicons.RepoForked
                              : ( repos.Private ? FontIconsService.Octicons.Lock
                                                : FontIconsService.Octicons.Repo );

        }

    }
}
