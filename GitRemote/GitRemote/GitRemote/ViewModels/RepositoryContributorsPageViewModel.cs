using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace GitRemote.ViewModels
{
    public class RepositoryContributorsPageViewModel : BindableBase, INavigationAware
    {
        public string Title => "Contributors";

        public ObservableCollection<RepositoryContributorModel> Contributors { get; set; }

        public RepositoryContributorsPageViewModel()
        {

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            if (!parameters.ContainsKey(nameof(Session)) ||
                !parameters.ContainsKey("OwnerName") ||
                !parameters.ContainsKey("ReposName"))
                return;

            var session = parameters[nameof(Session)] as Session;
            var ownerName = parameters["OwnerName"] as string;
            var reposName = parameters["ReposName"] as string;

            var manager = new RepositoryContributorsManager(session, ownerName, reposName);
            Contributors = new ObservableCollection<RepositoryContributorModel>
                (await manager.GetRepositoryContributors());
            RaisePropertyChanged(nameof(Contributors));
        }
    }
}
