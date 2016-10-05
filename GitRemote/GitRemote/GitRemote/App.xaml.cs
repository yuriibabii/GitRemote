using GitRemote.GitHub;
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
        protected override void OnInitialized()
        {
            InitializeComponent();
            //if User didn't exit from last session, then opens last session, otherwise opens start page
            NavigationService.NavigateAsync(StringService.CheckForNullOrEmpty(UserManager.GetLastUserFromStorage())
                ? $"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}"
                : $"{nameof(StartPage)}");
            //NavigationService.NavigateAsync($"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<BookmarksPage>();
            Container.RegisterTypeForNavigation<GistsPage>();
            Container.RegisterTypeForNavigation<IssueDashboardPage>();
            Container.RegisterTypeForNavigation<ReportAnIssuePage>();
            Container.RegisterTypeForNavigation<DetailPage>();
            Container.RegisterTypeForNavigation<FollowersPage>();
            Container.RegisterTypeForNavigation<FollowingPage>();
            Container.RegisterTypeForNavigation<LoginingPage>();
            Container.RegisterTypeForNavigation<MasterPage>();
            Container.RegisterTypeForNavigation<NewsPage>();
            Container.RegisterTypeForNavigation<ProfilePage>();
            Container.RegisterTypeForNavigation<RepositoriesPage>();
            Container.RegisterTypeForNavigation<StarsPage>();
            Container.RegisterTypeForNavigation<NavigationBarPage>();
            Container.RegisterTypeForNavigation<StartPage>();
            Container.RegisterTypeForNavigation<ChooseUserPage>();
            Container.RegisterTypeForNavigation<TwoFactorAuthPage>();
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            UserManager.SaveLastUser();
        }
    }
}
