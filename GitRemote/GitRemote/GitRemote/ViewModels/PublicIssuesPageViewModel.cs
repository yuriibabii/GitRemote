using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GitRemote.GitHub;
using Xamarin.Forms;
using static GitRemote.Services.MessageService;
using static GitRemote.Services.MessageService.MessageModels;

namespace GitRemote.ViewModels
{
    public class PublicIssuesPageViewModel : BindableBase
    {
        #region Commands
        public DelegateCommand AddCommand { get; }
        public DelegateCommand StarCommand { get; }
        public DelegateCommand ForkCommand { get; }
        public DelegateCommand ContributorsCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand OpenInBrowserCommand { get; }
        public DelegateCommand FilterCommand { get; }
        public DelegateCommand BookmarkCommand { get; }
        public DelegateCommand RefreshCommand { get; }
        #endregion

        private readonly INavigationService _navigationService;
        private readonly IDevice _device;
        public ObservableCollection<IssueModel> Issues { get; set; }
        private PublicIssuesManager _manager;

        private string _starText = string.Empty;
        public string StarText
        {
            get { return _starText; }
            set { SetProperty(ref _starText, value); }
        }

        private NavigationParameters _parameters;

        public PublicIssuesPageViewModel(INavigationService navigationService, IDevice device)
        {
            _navigationService = navigationService;
            _device = device;

            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
                (this, Messages.SendDataToPublicReposParticularPages, OnDataReceived);

            #region CommandsInitialization

            AddCommand = new DelegateCommand(OnAdd);
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
            _manager = new PublicIssuesManager(data.Session, data.OwnerName, data.ReposName);
            Issues = await GetPublicIssuesAsync();
            RaisePropertyChanged(nameof(Issues));

            StarText = await _manager.CheckStar()
             ? StarText = "Unstar"
             : StarText = "Star";

            _parameters = new NavigationParameters
            {
                {nameof(Session), data.Session },
                {"OwnerName", data.OwnerName },
                {"ReposName", data.ReposName }
            };
        }

        /// <summary>
        /// "Converts" task to observ collection
        /// </summary>
        /// <returns>Collection of issues</returns>
        private async Task<ObservableCollection<IssueModel>> GetPublicIssuesAsync()
        {
            return new ObservableCollection<IssueModel>
                (await _manager.GetPublicIssuesAsync());
        }

        #region ToolbarCommandHandlers

        private void OnAdd()
        {
            //Waits Implementation
        }

        private async void OnStar()
        {
            if (await _manager.CheckStar())
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
            var parameters = _parameters;
            parameters.Add("Type", "Issues");
            _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(FilterPage)}",
                parameters,
                animated: false);
        }

        private async void OnRefresh()
        {
            Issues = new ObservableCollection<IssueModel>(await _manager.GetPublicIssuesAsync());
            RaisePropertyChanged(nameof(Issues));
        }

        #endregion
    }
}
