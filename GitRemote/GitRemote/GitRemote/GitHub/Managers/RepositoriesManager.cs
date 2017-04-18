using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

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

                foreach (var repository in gitHubRepos)
                {
                    var repos = new RepositoryModel
                    {
                        Name = repository.Name,
                        Type = GetRepositoryType(repository),
                        OwnerName = repository.Owner.Login,
                        Description = repository.Description,
                        IsDescription = !string.IsNullOrEmpty(repository.Description),
                        Language = repository.Language ?? " ",
                        IsLanguage = repository.Language != null,
                        StarIcon = FontIconsService.Octicons.Star,
                        StarsCount = Convert.ToString(repository.StargazersCount),
                        ForkIcon = FontIconsService.Octicons.RepoForked,
                        ForksCount = Convert.ToString(repository.ForksCount)
                    };

                    gitRemoteRepos.Add(repos);
                }

                var groupedGitRemoteRepos = from model in gitRemoteRepos //foreach rep
                                            orderby model.Name // sort by RepoName
                                            group model by Convert.ToString(model.Name[0]).ToUpper() into modelGroup //Save each group and its key
                                            select new GroupingModel<string, RepositoryModel>(modelGroup.Key.ToUpper(), modelGroup); //Convert it to collection

                return new ObservableCollection<GroupingModel<string, RepositoryModel>>(groupedGitRemoteRepos);
            }
            catch (WebException ex)
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Getting repos from github failed! " + ex.Message);
            }
        }

        private string GetRepositoryType(Repository repos)
        {
            return repos.Fork ? "Fork"
                              : (repos.Private ? "Private"
                                                : "Public");
        }

    }
}
