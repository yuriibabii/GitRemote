using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MvvmHelpers;
using static GitRemote.Common.Enums;

namespace GitRemote.GitHub.Managers
{
    public class StarredRepositoriesManager
    {
        private readonly GitHubClient _gitHubClient;

        public StarredRepositoriesManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<ObservableRangeCollection<StarredRepositoryModel>> GetStarsAsync(int pageNumber = 1)
        {
            try
            {
                var options = new ApiOptions { PageCount = 1, PageSize = 20, StartPage = pageNumber };
                var gitHubStarredRepos = await _gitHubClient.Activity.Starring.GetAllForCurrent(options);
                var gitRemoteStarredRepos = new List<StarredRepositoryModel>();

                foreach (var repo in gitHubStarredRepos)
                {
                    var model = new StarredRepositoryModel
                    {
                        Type = GetRepositoryType(repo),
                        Description = repo.Description,
                        IsDescription = !string.IsNullOrEmpty(repo.Description),
                        Language = repo.Language ?? string.Empty,
                        IsLanguage = repo.Language != null,
                        StarsCount = Convert.ToString(repo.StargazersCount),
                        ForksCount = Convert.ToString(repo.ForksCount)
                    };

                    //Separating Full name to have repos name bold
                    var fullName = repo.FullName;
                    var separateIndex = fullName.LastIndexOf('/') + 1;
                    model.Name = fullName.Substring(separateIndex);
                    model.Path = fullName.Substring(0, separateIndex);
                    model.OwnerName = model.Path.Substring(0, model.Path.Length - 1);
                    gitRemoteStarredRepos.Add(model);
                }

                return new ObservableRangeCollection<StarredRepositoryModel>(gitRemoteStarredRepos);
            }
            catch (WebException ex)
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Getting starredRepos from github failed! " + ex.Message);
            }
        }

        private RepositoriesTypes GetRepositoryType(Repository repository)
        {
            if (repository.Fork) return RepositoriesTypes.Fork;
            if (repository.Private) return RepositoriesTypes.Private;

            return RepositoriesTypes.Public;
        }
    }
}
