using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using GitRemote.Views.MasterMenuPage;
using GitRemote.Views.PopUp;
using Prism.Navigation;
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

            var path = StringService.IsNullOrEmpty(UserManager.GetLastUserFromStorage())
                    ? $"{nameof(StartPage)}"
                    : $"{nameof(PrivateProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}";

            NavigationService.NavigateAsync(path);

            //NavigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(FilterPage)}");
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
            Container.RegisterTypeForNavigation<BranchSelectPage>();
            Container.RegisterTypeForNavigation<CommitsPage>();
            Container.RegisterTypeForNavigation<PublicIssuesPage>();
            Container.RegisterTypeForNavigation<PublicRepositoryPage>();
            Container.RegisterTypeForNavigation<PullRequestsPage>();
            Container.RegisterTypeForNavigation<ForkedRepositoryPage>();
            Container.RegisterTypeForNavigation<RepositoryContributorsPage>();
            Container.RegisterTypeForNavigation<FilterPage>();
            Container.RegisterTypeForNavigation<AssignedSelectPage>();
            Container.RegisterTypeForNavigation<MilestoneSelectPage>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            UserManager.SaveLastUser();
        }
    }
}
