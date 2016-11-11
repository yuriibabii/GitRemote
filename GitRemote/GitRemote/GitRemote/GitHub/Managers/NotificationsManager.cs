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
    public class NotificationsManager
    {
        private readonly GitHubClient _gitHubClient;

        public NotificationsManager(Session session)
        {
            _gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
               new InMemoryCredentialStore(new Credentials(session?.GetToken())));
        }

        public async Task<IEnumerable<NotificationModel>> GetNotificationsAsync()
        {
            try
            {
                var notifyRequest = new NotificationsRequest { All = true };
                var gitHubNotifies = await _gitHubClient.Activity.Notifications.GetAllForCurrent(notifyRequest);
                var gitRemoteNotifies = new List<NotificationModel>();

                foreach ( var notification in gitHubNotifies )
                {
                    var notify = new NotificationModel()
                    {
                        NotifyFullName = notification.Repository.FullName,
                        NotifyTitle = notification.Subject.Title,
                        NotifyTime = TimeService.ConvertToFriendly(notification.UpdatedAt),
                        IsRead = !notification.Unread,
                        NotifyTypeIcon = GetNotifyTypeIcon(notification)
                    };

                    gitRemoteNotifies.Add(notify);
                }

                return gitRemoteNotifies;
            }
            catch ( WebException ex )
            {
                throw new Exception("Something wrong with internet connection, try to On Internet " + ex.Message);
            }
            catch ( Exception ex )
            {
                throw new Exception("Getting notify from github failed! " + ex.Message);
            }
        }


        private string GetNotifyTypeIcon(Notification notification)
        {
            return notification.Subject.Type == "Issue"
                ? FontIconsService.Octicons.IssueOpened
                : ( notification.Subject.Type == "Commit"
                                ? FontIconsService.Octicons.Commit
                                : FontIconsService.Octicons.PullRequest );
        }
    }
}
