using GitRemote.GitHub;
using Prism.Events;
using Prism.Navigation;

namespace GitRemote.Services
{
    public static class MessageService
    {
        public static class Messages
        {
            public const string HardwareBackPressed = nameof(HardwareBackPressed);
            public const string TakePathPartsGrid = nameof(TakePathPartsGrid);
            public const string TakeBranchModelFromPopUpPage = nameof(TakeBranchModelFromPopUpPage);
            public const string SendManagerToBranchPopUpPage = nameof(SendManagerToBranchPopUpPage);
            public const string ScrollToActivatedBranchItem = nameof(ScrollToActivatedBranchItem);
            public const string SendDataToPublicReposParticularPages = nameof(SendDataToPublicReposParticularPages);
            public const string SetIsExecuteHardwareBack = nameof(SetIsExecuteHardwareBack);
            public const string SendManagerToFilterPopUp = nameof(SendManagerToFilterPopUp);
            public const string TakeAssigneeNameFromPopUpPage = nameof(TakeAssigneeNameFromPopUpPage);
            public const string TakeMilestoneNameFromPopUpPage = nameof(TakeMilestoneNameFromPopUpPage);
        }

        public static class MessageModels
        {
            public class SendDataToPublicReposParticularPagesModel
            {
                public Session Session { get; private set; }
                public string OwnerName { get; private set; }
                public string ReposName { get; private set; }

                public SendDataToPublicReposParticularPagesModel(Session session, string ownerName, string reposName)
                {
                    Session = session;
                    OwnerName = ownerName;
                    ReposName = reposName;
                }
            }

            public class DoNavigationModel
            {
                public string Path { get; private set; }
                public NavigationParameters Parameters { get; private set; }

                public DoNavigationModel(string path, NavigationParameters parameters)
                {
                    Path = path;
                    Parameters = parameters;
                }
            }
        }

        public class DoNavigation : PubSubEvent<MessageModels.DoNavigationModel> { }
        public class HideMasterPage : PubSubEvent<string> { }
        public class SetCurrentTabWithTitle : PubSubEvent<string> { }
        public class PublicReposCurrentTabChanged : PubSubEvent<string> { }
    }
}
