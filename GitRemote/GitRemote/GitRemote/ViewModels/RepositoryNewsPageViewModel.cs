using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class RepositoryNewsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public NotifyTask<ObservableCollection<RepositoryNewsModel>> News { get; }
        private readonly RepositoryNewsManager _manager;
        public GridLength ColumnWidth { get; set; }

        public RepositoryNewsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _manager = new RepositoryNewsManager();

            //News = NotifyTask.Create(GetRepositoryNewsAsync("UniorDev", "GitRemote"));
            News = NotifyTask.Create(GetRepositoryNewsAsync("gitrem2", "Created"));

            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);
        }

        private async Task<ObservableCollection<RepositoryNewsModel>> GetRepositoryNewsAsync(string login, string reposName)
        {
            return new ObservableCollection<RepositoryNewsModel>
                (await _manager.GetRepositoryNews(login, reposName));
        }
    }
}
