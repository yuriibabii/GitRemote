using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
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
    public class PrivateNewsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public NotifyTask<ObservableCollection<PrivateNewsModel>> PrivateNews { get; }
        private readonly PrivateNewsManager _privateNewsManager;

        public GridLength ColumnWidth { get; set; }

        public PrivateNewsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());

            _session = new Session(UserManager.GetLastUser(), store.Properties.First().Value,
                store.Properties["PrivateFeedUrl"]);

            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _privateNewsManager = new PrivateNewsManager(_session);

            PrivateNews = NotifyTask.Create(GetPrivateNewsAsync());

            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);

        }

        private async Task<ObservableCollection<PrivateNewsModel>> GetPrivateNewsAsync()
        {
            return new ObservableCollection<PrivateNewsModel>
                (await _privateNewsManager.GetPrivateNews());
        }
    }
}
