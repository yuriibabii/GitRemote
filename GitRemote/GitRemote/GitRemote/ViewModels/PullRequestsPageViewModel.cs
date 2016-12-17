using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class PullRequestsPageViewModel : BindableBase
    {

        #region Commands
        public DelegateCommand StarCommand { get; }
        public DelegateCommand ForkCommand { get; }
        public DelegateCommand ContributorsCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand OpenInBrowserCommand { get; }
        public DelegateCommand FilterCommand { get; }
        public DelegateCommand BookmarkCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        #endregion

        private INavigationService _navigationService;
        private readonly IDevice _device;
        private PullRequestsManager _manager;
        public ObservableCollection<PullRequestModel> PullRequests { get; set; }

        private string _starText = string.Empty;
        public string StarText
        {
            get { return _starText; }
            set { SetProperty(ref _starText, value); }
        }

        private NavigationParameters _parameters;

        public PullRequestsPageViewModel(INavigationService navigationService, IDevice device)
        {
            _navigationService = navigationService;
            _device = device;

            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
               (this, SendDataToPublicReposParticularPages, OnDataReceived);

            #region CommandsInitialization

            StarCommand = new DelegateCommand(OnStar);
            ForkCommand = new DelegateCommand(OnFork);
            ContributorsCommand = new DelegateCommand(OnContributors);
            ShareCommand = new DelegateCommand(OnShare);
            OpenInBrowserCommand = new DelegateCommand(OnOpenInBrowser);
            BookmarkCommand = new DelegateCommand(OnBookmark);
            FilterCommand = new DelegateCommand(OnFilter);
            RefreshCommand = new DelegateCommand(OnRefresh);

            #endregion

        }

        private async void OnDataReceived(SendDataToPublicReposParticularPagesModel data)
        {
            _manager = new PullRequestsManager(data.Session, data.OwnerName, data.ReposName);
            PullRequests = new ObservableCollection<PullRequestModel>(await _manager.GetPullRequestsAsync());
            OnPropertyChanged(nameof(PullRequests));
            MessagingCenter.Unsubscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages);

            StarText = await _manager.CheckStar()
               ? StarText = "Unstar"
               : StarText = "Star";

            _parameters = new NavigationParameters
            {
                {"Session", data.Session },
                {"OwnerName", data.OwnerName },
                {"ReposName", data.ReposName }
            };
        }

        #region ToolbarCommandHandlers

        private async void OnStar()
        {
            if ( await _manager.CheckStar() )
            {
                await _manager.UnstarRepository();
                StarText = "Star";
            }
            else
            {
                await _manager.StarRepository();
                StarText = "Unstar";
            }
        }

        private async void OnFork()
        {
            await _manager.ForkRepository();
        }

        private void OnContributors()
        {
            _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(RepositoryContributorsPage)}",
               _parameters,
               animated: false);
        }

        private async void OnShare()
        {
            await _manager.ShareLinkOnRepository();
        }

        private async void OnOpenInBrowser()
        {
            await _manager.OpenInBrowser(_device);
        }

        private void OnBookmark()
        {
            //Waits for implementation
        }

        private void OnFilter()
        {
            _parameters.Add("Type", "Pull Requests");
            _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(FilterPage)}",
                _parameters,
                animated: false);
            _parameters.Remove("Type");
        }

        private async void OnRefresh()
        {
            PullRequests = new ObservableCollection<PullRequestModel>(await _manager.GetPullRequestsAsync());
            OnPropertyChanged(nameof(PullRequests));
        }

        #endregion
    }
}
