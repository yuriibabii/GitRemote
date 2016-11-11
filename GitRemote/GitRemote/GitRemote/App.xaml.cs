using GitRemote.GitHub;
using GitRemote.Services;
using GitRemote.ViewModels;
using GitRemote.ViewModels.Authentication;
using GitRemote.ViewModels.MasterPageViews;
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
            ? $"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}"
            : $"{nameof(StartPage)}");
            //NavigationService.NavigateAsync($"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}");
            //NavigationService.NavigateAsync($"{nameof(RepositoriesPage)}");
            //NavigationService.NavigateAsync($"{nameof(StartPage)}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<BookmarksPage, BookmarksPageViewModel>();
            Container.RegisterTypeForNavigation<GistsPage, GistsPageViewModel>();
            Container.RegisterTypeForNavigation<IssueDashboardPage, IssueDashboardPageViewModel>();
            Container.RegisterTypeForNavigation<ReportAnIssuePage, ReportAnIssuePageViewModel>();
            Container.RegisterTypeForNavigation<DetailPage, DetailPageViewModel>();
            Container.RegisterTypeForNavigation<FollowPage, FollowPageViewModel>();
            Container.RegisterTypeForNavigation<LoginingPage, LoginingPageViewModel>();
            Container.RegisterTypeForNavigation<MasterPage, MasterPageViewModel>();
            Container.RegisterTypeForNavigation<NewsPage, NewsPageViewModel>();
            Container.RegisterTypeForNavigation<ProfilePage, ProfilePageViewModel>();
            Container.RegisterTypeForNavigation<RepositoriesPage, RepositoriesPageViewModel>();
            Container.RegisterTypeForNavigation<StarsPage, StarsPageViewModel>();
            Container.RegisterTypeForNavigation<NavigationBarPage>();
            Container.RegisterTypeForNavigation<StartPage, StartPageViewModel>();
            Container.RegisterTypeForNavigation<ChooseUserPage, ChooseUserPageViewModel>();
            Container.RegisterTypeForNavigation<TwoFactorAuthPage, TwoFactorAuthPageViewModel>();
            Container.RegisterTypeForNavigation<NotificationsPage, NotificationsPageViewModel>();
            Container.RegisterTypeForNavigation<GistsListPage, GistsListPageViewModel>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            UserManager.SaveLastUser();
        }
    }
}
