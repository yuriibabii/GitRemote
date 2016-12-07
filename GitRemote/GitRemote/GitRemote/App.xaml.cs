using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using GitRemote.Views.MasterPageViews;
using Prism.Unity;
using ChooseUserPage = GitRemote.Views.Authentication.ChooseUserPage;
using LoginingPage = GitRemote.Views.Authentication.LoginingPage;
using StartPage = GitRemote.Views.Authentication.StartPage;
using TwoFactorAuthPage = GitRemote.Views.Authentication.TwoFactorAuthPage;

namespace GitRemote
{
    public partial class App
    {
        public static int ScreenWidth = 0;

        protected override void OnInitialized()
        {
            InitializeComponent();

            //if User didn't exit from last session, then opens last session, otherwise opens start page
            NavigationService.NavigateAsync(StringService.CheckForNullOrEmpty(UserManager.GetLastUserFromStorage())
            ? $"{nameof(PrivateProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}"
            : $"{nameof(StartPage)}");

            
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<BookmarksPage>();
            Container.RegisterTypeForNavigation<GistsPage>();
            Container.RegisterTypeForNavigation<IssueDashboardPage>();
            Container.RegisterTypeForNavigation<DetailPage>();
            Container.RegisterTypeForNavigation<FollowPage>();
            Container.RegisterTypeForNavigation<LoginingPage>();
            Container.RegisterTypeForNavigation<MasterPage>();
            Container.RegisterTypeForNavigation<PrivateNewsPage>();
            Container.RegisterTypeForNavigation<PrivateProfilePage>();
            Container.RegisterTypeForNavigation<RepositoriesPage>();
            Container.RegisterTypeForNavigation<StarsPage>();
            Container.RegisterTypeForNavigation<NavigationBarPage>();
            Container.RegisterTypeForNavigation<StartPage>();
            Container.RegisterTypeForNavigation<ChooseUserPage>();
            Container.RegisterTypeForNavigation<TwoFactorAuthPage>();
            Container.RegisterTypeForNavigation<NotificationsPage>();
            Container.RegisterTypeForNavigation<GistsListPage>();
            Container.RegisterTypeForNavigation<RepositoryNewsPage>();
            Container.RegisterTypeForNavigation<FileExplorerPage>();
            Container.RegisterTypeForNavigation<SelectBranchPopUpPage>();
            Container.RegisterTypeForNavigation<CommitsPage>();
            Container.RegisterTypeForNavigation<PublicIssuesPage>();
            Container.RegisterTypeForNavigation<PublicRepositoryPage>();
            Container.RegisterTypeForNavigation<PullRequestsPage>();
            Container.RegisterTypeForNavigation<ForkedRepositoryPage>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            UserManager.SaveLastUser();
        }
    }
}
