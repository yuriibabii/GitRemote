using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class NewsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public NotifyTask<ObservableCollection<PrivateNewsModel>> PrivateNews { get; }
        private readonly PrivateNewsManager _privateNewsManager;
        private const int MaxNormalWidthForTitle = 450;
        private const int OtherWidth = 95;
        public GridLength ColumnWidth { get; set; }

        public NewsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());

            _session = new Session(UserManager.GetLastUser(), store.Properties.First().Value,
                store.Properties["PrivateFeedUrl"]);

            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _privateNewsManager = new PrivateNewsManager(_session);

            PrivateNews = NotifyTask.Create(GetPrivateNewsAsync());

            // It is doing to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < MaxNormalWidthForTitle
                ? App.ScreenWidth - OtherWidth
                : MaxNormalWidthForTitle);

        }

        private async Task<ObservableCollection<PrivateNewsModel>> GetPrivateNewsAsync()
        {
            return new ObservableCollection<PrivateNewsModel>
                (await _privateNewsManager.GetPrivateNews());
        }
    }
}
