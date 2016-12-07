using GitRemote.GitHub;

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
        }
    }
}
