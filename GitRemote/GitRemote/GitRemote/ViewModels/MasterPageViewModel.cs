using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Services;
using GitRemote.Views;
using GitRemote.Views.MasterPageViews;
using Octokit;
using Octokit.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using StartPage = GitRemote.Views.Authentication.StartPage;

namespace GitRemote.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {
        public string ProfileImageUrl
        {
            get { return _profileImageUrl; }
            set { SetProperty(ref _profileImageUrl, value); }
        }

        private string _profileImageUrl = "ic_account_circle_white_24dp.png";

        public string ProfileNickName
        {
            get { return _profileNickName; }
            set { SetProperty(ref _profileNickName, value); }
        }

        private string _profileNickName = "Unknown";

        private readonly NavigationParameters _navigationParameters;

        #region ImagePaths
        public string GistsPageImagePath => "ic_list_gists.png";
        public string DashboardPageImagePath => "ic_list_issueDashboard.png";
        public string BookmarksPageImagePath => "ic_list_bookmarks.png";
        public string IssuePageImagePath => "ic_list_issue.png";
        #endregion

        #region Commands
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand GistsCommand { get; }
        public DelegateCommand DashboardCommand { get; }
        public DelegateCommand BookmarksCommand { get; }
        public DelegateCommand IssueCommand { get; }
        #endregion

        private readonly INavigationService _navigationService;
        private readonly Session _session;
        private readonly IMetricsHelper _metricsHelper;

        public MasterPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider,
            IMetricsHelper metricsHelper)
        {
            #region Initialize Commands

            ExitCommand = new DelegateCommand(OnExitTapped);
            GistsCommand = new DelegateCommand(OnGistsTapped);
            DashboardCommand = new DelegateCommand(OnDashboardTapped);
            BookmarksCommand = new DelegateCommand(OnBookmarksTapped);
            IssueCommand = new DelegateCommand(OnIssueTapped);
            #endregion

            _navigationService = navigationService;
            _metricsHelper = metricsHelper;
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            _navigationParameters = new NavigationParameters { { "Session", _session } };
            SetProfileImageAndNickNameAsync();
        }


        /// <summary>
        /// Creates gitHubClient with existing token, gets Avatar from url and nickname
        /// </summary>
        private async void SetProfileImageAndNickNameAsync()
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(_session?.GetToken())));
            try
            {
                var user = await gitHubClient.User.Current();
                ProfileImageUrl = user?.AvatarUrl;
                ProfileNickName = StringService.CheckForNullOrEmpty(user?.Name)
                    ? user?.Name
                    : user?.Login;
            }
            catch ( WebException )
            {
                Debug.WriteLine("Something wrong with internet connection, try to On Internet");
            }
            catch ( Exception )
            {
                Debug.WriteLine("Getting user from github failed!");
            }
        }

        #region CommandHandlers

        private async void OnGistsTapped()
        {
            GistsManager.SetTabsTitles(_metricsHelper);
            await _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(GistsPage)}",
                _navigationParameters, animated: false);
        }

        private async void OnDashboardTapped()
        {
            await _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(IssueDashboardPage)}",
                _navigationParameters, animated: false);
        }

        private async void OnBookmarksTapped()
        {
            await _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(BookmarksPage)}",
                _navigationParameters, animated: false);
        }

        private void OnIssueTapped()
        {
            if ( !_navigationParameters.ContainsKey("OwnerName") )
                _navigationParameters.Add("OwnerName", "UniorDev");

            if ( !_navigationParameters.ContainsKey("ReposName") )
                _navigationParameters.Add("ReposName", "GitRemote");

            //_navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(PublicRepositoryPage)}",
            //   _navigationParameters, animated: false);

            var path = $"{nameof(PublicRepositoryPage)}";

            MessagingCenter.Send(new DoNavigationModel(path, _navigationParameters), DoNavigation);
            MessagingCenter.Send("Issues", SetCurrentTabWithTitle);
            MessagingCenter.Send("JustIgnore", HideMasterPage);
        }

        private async void OnExitTapped()
        {
            UserManager.SetLastUser(string.Empty);
            var navigationStack = new Uri("https://Necessary/" + $"{nameof(StartPage)}", UriKind.Absolute);
            await _navigationService.NavigateAsync(navigationStack, animated: false);
        }


        #endregion

    }
}
