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
using static GitRemote.Common.Enums.NotificationsTypes;

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

        public async Task<ObservableRangeCollection<NotificationModel>> GetNotificationsAsync(int pageNumber = 1)
        {
            try
            {
                var options = new ApiOptions { PageCount = 1, PageSize = 20, StartPage = pageNumber };
                var notifyRequest = new NotificationsRequest { All = true };
                var gitHubNotifies = await _gitHubClient.Activity.Notifications.GetAllForCurrent(notifyRequest, options);
                var gitRemoteNotifies = new List<NotificationModel>();

                foreach ( var notification in gitHubNotifies )
                {
                    var notify = new NotificationModel()
                    {
                        FullName = notification.Repository.FullName,
                        Title = notification.Subject.Title,
                        Time = TimeService.ConvertToFriendly(notification.UpdatedAt),
                        IsRead = !notification.Unread,
                        TypeIcon = GetNotifyTypeIcon(notification)
                    };

                    gitRemoteNotifies.Add(notify);
                }

                return new ObservableRangeCollection<NotificationModel>(gitRemoteNotifies);
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
            return notification.Subject.Type == nameof(NotificationsTypes.Issue)
                ? FontIconsService.Octicons.IssueOpened
                : ( notification.Subject.Type == nameof(NotificationsTypes.Commit)
                                ? FontIconsService.Octicons.Commit
                                : FontIconsService.Octicons.PullRequest );
        }
    }
}
