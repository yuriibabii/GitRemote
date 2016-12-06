using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
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
        private INavigationService _navigationService;
        public ObservableCollection<RepositoryNewsModel> News { get; set; }
        private RepositoryNewsManager _manager;
        public GridLength ColumnWidth { get; set; }

        public RepositoryNewsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);

            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages, OnDataReceived);
        }

        private async void OnDataReceived(SendDataToPublicReposParticularPagesModel data)
        {
            _manager = new RepositoryNewsManager(data.Session, data.OwnerName, data.ReposName);

            var ownerName = data.OwnerName;
            var reposName = data.ReposName;

            News = await GetRepositoryNewsAsync();
            OnPropertyChanged(nameof(News));
            MessagingCenter.Unsubscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages);
        }

        private async Task<ObservableCollection<RepositoryNewsModel>> GetRepositoryNewsAsync()
        {
            return new ObservableCollection<RepositoryNewsModel>
                (await _manager.GetRepositoryNews());
        }

    }
}
