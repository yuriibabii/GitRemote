using GitRemote.DI;
using GitRemote.Models;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Plugin.Share.Abstractions;

namespace GitRemote.GitHub.Managers
{
    public class CommitsManager
    {
        private readonly GitHubClient _gitHubClient;
        public string CurrentBranch { get; private set; }
        private Repository _currentRepo;
        private readonly string _ownerName;
        private readonly string _reposName;

        public CommitsManager(Session session, string ownerName, string reposName)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session.GetToken())));
            _ownerName = ownerName;
            _reposName = reposName;
        }

        public async Task SetCurrentRepo()
        {
            _currentRepo = await _gitHubClient.Repository.Get(_ownerName, _reposName);
        }

        public void SetCurrentBranch(string branch)
        {
            if (!StringService.IsNullOrEmpty(branch))
                CurrentBranch = branch;
        }

        public void SetDefaultBranch()
        {
            CurrentBranch = _currentRepo.DefaultBranch;
        }

        public async Task<IEnumerable<CommitModel>> GetCommitsAsync()
        {
            try
            {
                var options = new ApiOptions { PageSize = 30, PageCount = 1 };
                var request = new CommitRequest { Sha = CurrentBranch };

                var gitHubCommitsItems = await _gitHubClient.Repository.Commit.GetAll
                    (_currentRepo.Owner.Login, _currentRepo.Name, request, options);

                var gitRemoteCommitsItems = new List<CommitModel>();

                foreach (var item in gitHubCommitsItems)
                {
                    var commitModel = new CommitModel
                    {
                        Id = item.Sha.Substring(0, 10),
                        AvatarImageUrl = item.Author?.AvatarUrl,
                        Title = item.Commit.Message,
                        OwnerName = item.Author?.Login ?? item.Commit.Author.Name,
                        CreatedTime = TimeService.ConvertToFriendly(item.Commit.Author.Date.ToString()),
                        CommentsCount = item.Commit.CommentCount,
                        IsCommented = item.Commit.CommentCount > 0
                    };

                    gitRemoteCommitsItems.Add(commitModel);
                }

                return gitRemoteCommitsItems;
            }
            catch (WebException ex)
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Getting commits from github failed! " + ex.Message);
            }
        }

        public async Task<IReadOnlyList<Branch>> GetBranchesAsync()
        {
            return await _gitHubClient.Repository.Branch.GetAll(_currentRepo.Owner.Login, _currentRepo.Name);
        }

        public async Task<IReadOnlyList<RepositoryTag>> GetTagsAsync()
        {
            return await _gitHubClient.Repository.GetAllTags(_currentRepo.Owner.Login, _currentRepo.Name);
        }

        public async Task<bool> CheckStar()
        {
            return await _gitHubClient.Activity.Starring.CheckStarred(_ownerName, _reposName);
        }

        public async Task StarRepository()
        {
            await _gitHubClient.Activity.Starring.StarRepo(_ownerName, _reposName);
        }

        public async Task UnstarRepository()
        {
            await _gitHubClient.Activity.Starring.RemoveStarFromRepo(_ownerName, _reposName);
        }

        public async Task ForkRepository()
        {
            await _gitHubClient.Repository.Forks.Create(_ownerName, _reposName, new NewRepositoryFork());
        }

        public async Task OpenInBrowser(IDevice device)
        {
            await device.LaunchUriAsync(new Uri($"{ConstantsService.GitHubOfficialPageUrl}{_ownerName}/{_reposName}"));
        }

        public async Task ShareLinkOnRepository()
        {
            var message = new ShareMessage { Url = $"{ConstantsService.GitHubOfficialPageUrl}{_ownerName}/{_reposName}" };
            await CrossShare.Current.Share(message);
        }
    }
}
