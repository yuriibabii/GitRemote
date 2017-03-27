using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class RepositoryNewsPageViewModel : BindableBase
    {
        #region Commands
        public DelegateCommand StarCommand { get; }
        public DelegateCommand ForkCommand { get; }
        public DelegateCommand ContributorsCommand { get; }
        public DelegateCommand ShareCommand { get; }
        public DelegateCommand OpenInBrowserCommand { get; }
        #endregion

        private readonly INavigationService _navigationService;
        public ObservableCollection<RepositoryNewsModel> News { get; set; }
        private RepositoryNewsManager _manager;
        public GridLength ColumnWidth { get; set; }
        private readonly IDevice _device;
        private string _starText = string.Empty;
        private NavigationParameters _parameters;
        public string StarText
        {
            get { return _starText; }
            set { SetProperty(ref _starText, value); }
        }

        public RepositoryNewsPageViewModel(INavigationService navigationService, IDevice device)
        {
            _navigationService = navigationService;
            _device = device;
            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);

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
            _manager = new RepositoryNewsManager(data.Session, data.OwnerName, data.ReposName);

            News = await GetRepositoryNewsAsync();
            OnPropertyChanged(nameof(News));
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

        private async Task<ObservableCollection<RepositoryNewsModel>> GetRepositoryNewsAsync()
        {
            return new ObservableCollection<RepositoryNewsModel>
                (await _manager.GetRepositoryNews());
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

    }
}
