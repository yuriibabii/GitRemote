using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GitRemote.Models;
using GitRemote.Services;
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

        public async Task<IEnumerable<FollowModel>> GetFollowsAsync()
        {
            try
            {
                IEnumerable<User> gitHubFollowUsers;

                //Switch Follow page
                if (IsActiveFollowersPage)
                {
                    IsActiveFollowersPage = !IsActiveFollowersPage;
                    gitHubFollowUsers = await _gitHubClient.User.Followers.GetAllForCurrent();
                }
                else
                {
                    IsActiveFollowersPage = !IsActiveFollowersPage;
                    gitHubFollowUsers = await _gitHubClient.User.Followers.GetAllFollowingForCurrent();
                }

                var gitRemoteFollowUsers = new List<FollowModel>();

                foreach (var user in gitHubFollowUsers)
                {
                    var followItem = new FollowModel()
                    {
                        Name = StringService.IsNullOrEmpty(user?.Name) ? user?.Login : user?.Name,
                        ImageUrl = user?.AvatarUrl
                    };

                    gitRemoteFollowUsers.Add(followItem);
                }

                return gitRemoteFollowUsers;
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
