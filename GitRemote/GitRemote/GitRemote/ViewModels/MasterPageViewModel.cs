using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using Octokit;
using Octokit.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using GitRemote.Views.MasterMenuPage;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Services.ConstantsService;
using static GitRemote.Services.MessageService;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using static GitRemote.Services.StringService.SoftStrings;
using StartPage = GitRemote.Views.Authentication.StartPage;

namespace GitRemote.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {
        #region Properties

        private string _profileImageUrl;
        public string ProfileImageUrl
        {
            get { return _profileImageUrl; }
            set { SetProperty(ref _profileImageUrl, value); }
        }

        private string _profileNickName = "Unknown";
        public string ProfileNickName
        {
            get { return _profileNickName; }
            set { SetProperty(ref _profileNickName, value); }
        }

        public string GistsFontIcon => FontIconsService.Octicons.Gist;
        public string SignOutFontIcon => FontIconsService.Octicons.SignOut;
        public string DashboardFontIcon => FontIconsService.Octicons.Dashboard;
        public string BookmarksFontIcon => FontIconsService.Octicons.Bookmark;
        public string IssueFontIcon => FontIconsService.Octicons.IssueOpened;

        #endregion

        #region Commands
        public DelegateCommand SignOutCommand => new DelegateCommand(OnSignOut);
        public DelegateCommand GistsCommand => new DelegateCommand(OnGists);
        public DelegateCommand DashboardCommand => new DelegateCommand(OnDashboard);
        public DelegateCommand BookmarksCommand => new DelegateCommand(OnBookmarks);
        public DelegateCommand IssueCommand => new DelegateCommand(OnIssue);
        #endregion

        private readonly NavigationParameters _navigationParameters;
        private readonly INavigationService _navigationService;
        private readonly Session _session;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMetricsHelper _metricsHelper;

        public MasterPageViewModel(INavigationService navigationService,
            ISecuredDataProvider securedDataProvider,
            IEventAggregator eventAggregator,
            IMetricsHelper metricsHelper)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;
            _metricsHelper = metricsHelper;
            var token = securedDataProvider.Retreive(ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            _navigationParameters = new NavigationParameters { { nameof(Session), _session } };
            SetProfileImageAndNickNameAsync();
        }

        private async void SetProfileImageAndNickNameAsync()
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue(AppName),
                new InMemoryCredentialStore(new Credentials(_session?.GetToken())));
            try
            {
                var user = await gitHubClient.User.Current();
                if ( user == null ) return;

                ProfileImageUrl = user.AvatarUrl;
                ProfileNickName = StringService.IsNullOrEmpty(user.Name)
                    ? user.Login
                    : user.Name;
            }
            catch ( WebException )
            {
                Debug.WriteLine("Something wrong with internet connection, try to On Internet");
            }
            catch ( Exception exception )
            {
                Debug.WriteLine("Getting user from github failed! " + exception.Message);
            }
        }

        private void OnGists()
        {
            GistsManager.SetTabsTitles(_metricsHelper);

            var path = $"{nameof(GistsPage)}";

            _eventAggregator
                .GetEvent<DoNavigation>()
                .Publish(new DoNavigationModel(path, _navigationParameters));

            _eventAggregator
                .GetEvent<HideMasterPage>()
                .Publish(IgnoreString);
        }

        private void OnDashboard()
        {
            var path = $"{nameof(IssueDashboardPage)}";

            _eventAggregator
                .GetEvent<DoNavigation>()
                .Publish(new DoNavigationModel(path, _navigationParameters));

            _eventAggregator
                .GetEvent<HideMasterPage>()
                .Publish(IgnoreString);
        }

        private void OnBookmarks()
        {
            var path = $"{nameof(BookmarksPage)}";

            _eventAggregator
                 .GetEvent<DoNavigation>()
                 .Publish(new DoNavigationModel(path, _navigationParameters));

            _eventAggregator
                .GetEvent<HideMasterPage>()
                .Publish(IgnoreString);
        }

        private void OnIssue()
        {
            if ( !_navigationParameters.ContainsKey(OwnerName) )
                _navigationParameters.Add(OwnerName, GitHubUserName);

            if ( !_navigationParameters.ContainsKey(ReposName) )
                _navigationParameters.Add(ReposName, AppName);

            var path = $"{nameof(PublicRepositoryPage)}";

            _eventAggregator
                .GetEvent<DoNavigation>()
                .Publish(new DoNavigationModel(path, _navigationParameters));

            _eventAggregator
                .GetEvent<SetCurrentTabWithTitle>()
                .Publish(Issues);

            _eventAggregator
                .GetEvent<HideMasterPage>()
                .Publish(IgnoreString);
        }

        private async void OnSignOut()
        {
            UserManager.SetLastUser(string.Empty);
            var navigationStack = new Uri("https://Necessary/" + $"{nameof(StartPage)}", UriKind.Absolute);
            await _navigationService.NavigateAsync(navigationStack, animated: false);
        }
    }
}
