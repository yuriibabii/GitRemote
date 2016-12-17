using GitRemote.GitHub;
using Prism.Navigation;

namespace GitRemote.Services
{
    public static class MessageService
    {
        public static class Messages
        {
            public const string HardwareBackPressed = "HardwareBackPressed";
            public const string TakePathPartsGrid = "TakePathPartsGrid";
            public const string TakeBranchModelFromPopUpPage = "TakeBranchModelFromPopUpPage";
            public const string SendManagerToBranchPopUpPage = "SendManagerToBranchPopUpPage";
            public const string ScrollToActivatedBranchItem = "ScrollToActivatedBranchItem";
            public const string SendDataToPublicReposParticularPages = "SendDataToPublicReposParticularPages";
            public const string SetIsExecuteHardwareBack = "SetIsExecuteHardwareBack";
            public const string PublicReposCurrentTabChanged = "PublicReposCurrentTabChanged";
            public const string SetCurrentTabWithTitle = "SetCurrentTabWithTitle";
            public const string DoNavigation = "DoNavigation";
            public const string HideMasterPage = "HideMasterPage";
            public const string SendManagerToFilterPage = "SendManagerToFilterPage";
            public const string TakeAssigneeNameFromPopUpPage = "TakeAssigneeNameFromPopUpPage";
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
    }
}
