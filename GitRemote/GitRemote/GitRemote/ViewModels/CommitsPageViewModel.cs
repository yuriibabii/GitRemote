using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using static System.String;

namespace GitRemote.ViewModels
{
    public class CommitsPageViewModel : BindableBase, INavigationAware
    {
        #region Commands
        public DelegateCommand StarCommand { get; }
        public DelegateCommand ForkCommand { get; }
        public DelegateCommand ContributorsCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand OpenInBrowserCommand { get; }
        #endregion

        private INavigationService _navigationService;
        private readonly IDevice _device;
        private CommitsManager _manager;
        public DelegateCommand BotPanelTapped { get; }
        public ObservableCollection<CommitModel> Commits { get; set; }
        public string BranchIcon => _currentSourceType == "Branch"
            ? FontIconsService.Octicons.Branch
            : FontIconsService.Octicons.Tag;

        public NotifyTask SetCurrentRepoTask;

        private string _currentSourceType = "Branch";

        private string _currentBranch = Empty;
        public string CurrentBranch
        {
            get { return _currentBranch; }
            set { SetProperty(ref _currentBranch, value); }
        }

        private string _starText = Empty;
        public string StarText
        {
            get { return _starText; }
            set { SetProperty(ref _starText, value); }
        }

        private NavigationParameters _parameters;

        public CommitsPageViewModel(INavigationService navigationService, IDevice device)
        {
            _navigationService = navigationService;
            _device = device;
            BotPanelTapped = new DelegateCommand(OnBotPanelTapped);
            MessagingCenter.Subscribe<SelectBranchPopUpModel>(this, TakeBranchModelFromPopUpPage, OnBranchSelected);
            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages, OnDataReceived);

            StarCommand = new DelegateCommand(OnStar);
            ForkCommand = new DelegateCommand(OnFork);
            ContributorsCommand = new DelegateCommand(OnContributors);
            ShareCommand = new DelegateCommand(OnShare);
            OpenInBrowserCommand = new DelegateCommand(OnOpenInBrowser);
        }

        private async void OnDataReceived(SendDataToPublicReposParticularPagesModel data)
        {
            MessagingCenter.Unsubscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages);

            _manager = new CommitsManager(data.Session, data.OwnerName, data.ReposName);
            var task = _manager.SetCurrentRepo();
            await task.ContinueWith(t => _manager.SetDefaultBranch());
            CurrentBranch = _manager.CurrentBranch;
            Commits = await GetCommitsAsync();
            OnPropertyChanged(nameof(Commits));
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

        private async void OnBranchSelected(SelectBranchPopUpModel selectBranchPopUpModel)
        {
            _currentSourceType = selectBranchPopUpModel.Type;
            OnPropertyChanged(nameof(BranchIcon));
            _manager.SetCurrentBranch(selectBranchPopUpModel.Name);
            CurrentBranch = _manager.CurrentBranch;
            Commits = await GetCommitsAsync();
            OnPropertyChanged(nameof(Commits));
        }

        private void OnBotPanelTapped()
        {
            PopupNavigation.PushAsync(new SelectBranchPopUpPage());
            MessagingCenter.Send(_manager, SendManagerToBranchPopUpPage);
        }

        private async Task<ObservableCollection<CommitModel>> GetCommitsAsync()
        {
            return new ObservableCollection<CommitModel>
                (await _manager.GetCommitsAsync());
        }

        #region CommandHandlers

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

        #endregion

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<SelectBranchPopUpModel>(this, TakeBranchModelFromPopUpPage);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
