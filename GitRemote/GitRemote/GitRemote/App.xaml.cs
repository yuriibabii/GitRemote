using GitRemote.GitHub;
using GitRemote.Views;
using GitRemote.Views.MasterPageViews;
using Prism.Unity;

namespace GitRemote
{
    public partial class App
    {
        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync($"{nameof(StartPage)}");
           // NavigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(TwoFactorAuthPage)}");
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
