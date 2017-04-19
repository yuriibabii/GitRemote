using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GitRemote.Models;
using GitRemote.Services;
using MvvmHelpers;
using Octokit;
using Octokit.Internal;

namespace GitRemote.GitHub.Managers
{
    public class FollowManager
    {
        public static bool IsActiveFollowersPage = true;

        private readonly GitHubClient _gitHubClient;

        public FollowManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<ObservableRangeCollection<FollowModel>> GetFollowsAsync(int pageNumber = 1)
        {
            try
            {
                IEnumerable<User> gitHubFollowUsers;
                var options = new ApiOptions { PageCount = 1, PageSize = 20, StartPage = pageNumber };

                //Switch Follow page
                if (IsActiveFollowersPage)
                {
                    IsActiveFollowersPage = !IsActiveFollowersPage;
                    gitHubFollowUsers = await _gitHubClient.User.Followers.GetAllForCurrent(options);
                }
                else
                {
                    IsActiveFollowersPage = !IsActiveFollowersPage;
                    gitHubFollowUsers = await _gitHubClient.User.Followers.GetAllFollowingForCurrent(options);
                }

                var gitRemoteFollowUsers = new List<FollowModel>();

                foreach (var user in gitHubFollowUsers)
                {
                    var followItem = new FollowModel()
                    {
                        Name = StringService.IsNullOrEmpty(user.Name) ? user.Login : user.Name,
                        ImageUrl = user.AvatarUrl
                    };

                    gitRemoteFollowUsers.Add(followItem);
                }

                return new ObservableRangeCollection<FollowModel>(gitRemoteFollowUsers);
            }

            catch (WebException ex)
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Getting follow from github failed! " + ex.Message);
            }
        }
    }
}
