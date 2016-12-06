using GitRemote.GitHub.Managers;
using GitRemote.Models;
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
        private INavigationService _navigationService;
        private PullRequestsManager _manager;
        public ObservableCollection<PullRequestModel> PullRequests { get; set; }

        public PullRequestsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
               (this, SendDataToPublicReposParticularPages, OnDataReceived);
        }

        private async void OnDataReceived(SendDataToPublicReposParticularPagesModel data)
        {
            _manager = new PullRequestsManager(data.Session, data.OwnerName, data.ReposName);
            PullRequests = new ObservableCollection<PullRequestModel>(await _manager.GetPullRequestsAsync());
            MessagingCenter.Unsubscribe<SendDataToPublicReposParticularPagesModel>
                (this, SendDataToPublicReposParticularPages);
        }
    }
}
