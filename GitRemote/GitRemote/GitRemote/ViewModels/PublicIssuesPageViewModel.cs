using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using static GitRemote.Services.MessageService;
using static GitRemote.Services.MessageService.MessageModels;

namespace GitRemote.ViewModels
{
    public class PublicIssuesPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public ObservableCollection<IssueModel> Issues { get; set; }
        private PublicIssuesManager _manager;

        public PublicIssuesPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;



            MessagingCenter.Subscribe<SendDataToPublicReposParticularPagesModel>
                (this, Messages.SendDataToPublicReposParticularPages, OnDataReceived);
        }

        private async void OnDataReceived(SendDataToPublicReposParticularPagesModel data)
        {
            _manager = new PublicIssuesManager(data.Session, data.OwnerName, data.ReposName);
            Issues = await GetPublicIssuesAsync();
            OnPropertyChanged(nameof(Issues));
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
    }
}
