using GitRemote.GitHub;
using GitRemote.Services;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class PublicRepositoryPageViewModel : BindableBase, INavigationAware
    {
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
            if ( !parameters.ContainsKey("Session") ||
                !parameters.ContainsKey("OwnerName") ||
                !parameters.ContainsKey("ReposName") )
                return;

            var session = parameters["Session"] as Session;
            var ownerName = parameters["OwnerName"] as string;
            var reposName = parameters["ReposName"] as string;

            var model = new SendDataToPublicReposParticularPagesModel(session, ownerName, reposName);

            MessagingCenter.Send(model, SendDataToPublicReposParticularPages);
        }
    }
}
