using GitRemote.GitHub;
using GitRemote.Views.MasterPageViews;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using StartPage = GitRemote.Views.Authentication.StartPage;

namespace GitRemote.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {
        #region Constants
        //private const string GistsPageImagePath = "ic_code_black_24dp.png";
        //private const string IssueDashboardPageImagePath = "ic_slow_motion_video_black_24dp.png";
        //private const string BookmarksPageImagePath = "ic_bookmark_black_24dp.png";
        //private const string ReportAnIssuePageImagePath = "ic_error_outline_black_24dp.png";
        public string GistsPageImagePath => "ic_list_gists.png";
        public string IssueDashboardPageImagePath => "ic_list_issueDashboard.png";
        public string BookmarksPageImagePath => "ic_list_bookmarks.png";
        public string ReportAnIssuePageImagePath => "ic_list_issue.png";
        #endregion

        private readonly INavigationService _navigationService;

        #region Commands
        public DelegateCommand ExitCommand { get; }
        public DelegateCommand GistsCommand { get; }
        public DelegateCommand DashboardCommand { get; }
        public DelegateCommand BookmarksCommand { get; }
        public DelegateCommand IssueCommand { get; }
        #endregion

        public MasterPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            #region Initialize Commands
            ExitCommand = new DelegateCommand(OnExitTapped);
            GistsCommand = new DelegateCommand(OnGistsTapped);
            DashboardCommand = new DelegateCommand(OnDashboardTapped);
            BookmarksCommand = new DelegateCommand(OnBookmarksTapped);
            IssueCommand = new DelegateCommand(OnIssueTapped);
            #endregion



        }

        private void OnIssueTapped()
        {
            _navigationService.NavigateAsync($"{nameof(ReportAnIssuePage)}");
        }

        private void OnBookmarksTapped()
        {
            _navigationService.NavigateAsync($"{nameof(BookmarksPage)}");
        }

        private void OnDashboardTapped()
        {
            _navigationService.NavigateAsync($"{nameof(IssueDashboardPage)}");
        }

        private void OnGistsTapped()
        {
            _navigationService.NavigateAsync($"{nameof(ReportAnIssuePage)}");
        }

        private void OnExitTapped()
        {
            UserManager.SetLastUser(string.Empty);
            var navigationStack = new Uri("https://Necessary/" + $"{nameof(StartPage)}", UriKind.Absolute);
            _navigationService.NavigateAsync(navigationStack, animated: false);
        }
    }
}
