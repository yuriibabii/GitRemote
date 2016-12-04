using GitRemote.DI;
using GitRemote.GitHub;
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
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class CommitsPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public DelegateCommand BotPanelTapped { get; }
        public NotifyTask<ObservableCollection<CommitModel>> Commits { get; set; }
        private readonly CommitsManager _commitsManager;
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

        public CommitsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());

            _session = new Session(UserManager.GetLastUser(), store.Properties.First().Value,
                store.Properties["PrivateFeedUrl"]);

            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _commitsManager = new CommitsManager(_session);
            //SetCurrentRepoTask = NotifyTask.Create(_commitsManager.SetCurrentRepo("UniorDev", "GitRemote"));
            SetCurrentRepoTask = NotifyTask.Create(_commitsManager.SetCurrentRepo("UniorDev", "ForkHub"));
            SetCurrentRepoTask.TaskCompleted.ContinueWith(repoTask =>
            {
                _commitsManager.SetDefaultBranch();
                CurrentBranch = _commitsManager.CurrentBranch;
                Commits = NotifyTask.Create(GetCommitsAsync);
                OnPropertyChanged(nameof(Commits));
            });

            BotPanelTapped = new DelegateCommand(OnBotPanelTapped);
            MessagingCenter.Subscribe<SelectBranchPopUpModel>(this, ConstantsService.Messages.TakeBranchModelFromPopUpPage, OnBranchSelected);
        }

        private void OnBranchSelected(SelectBranchPopUpModel selectBranchPopUpModel)
        {
            _currentSourceType = selectBranchPopUpModel.Type;
            OnPropertyChanged(nameof(BranchIcon));
            _commitsManager.SetCurrentBranch(selectBranchPopUpModel.Name);
            CurrentBranch = _commitsManager.CurrentBranch;
            Commits = NotifyTask.Create(GetCommitsAsync);
            OnPropertyChanged(nameof(Commits));
        }

        private void OnBotPanelTapped()
        {
            PopupNavigation.PushAsync(new SelectBranchPopUpPage());
            MessagingCenter.Send(_commitsManager, ConstantsService.Messages.SendManagerToBranchPopUpPage);
        }

        private async Task<ObservableCollection<CommitModel>> GetCommitsAsync()
        {
            return new ObservableCollection<CommitModel>
                (await _commitsManager.GetCommitsAsync());
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<SelectBranchPopUpModel>(this, ConstantsService.Messages.TakeBranchModelFromPopUpPage);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }
    }
}
