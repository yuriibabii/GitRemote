using GitRemote.GitHub;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class PublicRepositoryPageViewModel : BindableBase, INavigationAware
    {
        private string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _subTitle = string.Empty;

        public string SubTitle
        {
            get { return _subTitle; }
            set { SetProperty(ref _subTitle, value); }
        }

        private string _avatarUrl = string.Empty;

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { SetProperty(ref _avatarUrl, value); }
        }

        public PublicRepositoryPageViewModel()
        {

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey(nameof(Session)) ||
                !parameters.ContainsKey("OwnerName") ||
                !parameters.ContainsKey("ReposName") )
                return;

            var session = parameters[nameof(Session)] as Session;
            var ownerName = parameters["OwnerName"] as string;
            var reposName = parameters["ReposName"] as string;

            Title = reposName;
            SubTitle = ownerName;

            var client = new UsersClient
                (new ApiConnection
                (new Connection
                (new ProductHeaderValue
                (ConstantsService.AppName), new InMemoryCredentialStore
                (new Credentials(session?.GetToken())))));

            var owner = client.Get(ownerName);

            AvatarUrl = owner.Result.AvatarUrl; // Blocks task, can be dangerous

            var model = new SendDataToPublicReposParticularPagesModel(session, ownerName, reposName);

            MessagingCenter.Send(model, SendDataToPublicReposParticularPages);

        }
    }
}
