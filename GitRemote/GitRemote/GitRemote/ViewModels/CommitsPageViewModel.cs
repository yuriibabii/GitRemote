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

namespace GitRemote.ViewModels
{
    public class CommitsPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private CommitsManager _manager;
        public DelegateCommand BotPanelTapped { get; }
        public ObservableCollection<CommitModel> Commits { get; set; }
        public string BranchIcon => _currentSourceType == "Branch"
            ? FontIconsService.Octicons.Branch
            : FontIconsService.Octicons.Tag;

        public NotifyTask SetCurrentRepoTask;

        private string _currentSourceType = "Branch";

        public string CurrentBranch
        {
            get { return _currentBranch; }
            set { SetProperty(ref _currentBranch, value); }
        }

        private string _currentBranch = string.Empty;

        public CommitsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            BotPanelTapped = new DelegateCommand(OnBotPanelTapped);
            MessagingCenter.Subscribe<SelectBranchPopUpModel>(this, TakeBranchModelFromPopUpPage, OnBranchSelected);
            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages, OnDataReceived);
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
