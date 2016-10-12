using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;

namespace GitRemote.ViewModels
{
    public class NewsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        private readonly Session _session;
        public ObservableCollection<PrivateNewsModel> PrivateNews { get; }
        private readonly PrivateNewsManager _privateNewsManager;

        public NewsPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), store.Properties.First().Value, store.Properties["PrivateFeedUrl"]);

            var navigationParameters = new NavigationParameters { { "Session", _session } };

            _privateNewsManager = new PrivateNewsManager(_session);

            PrivateNews = _privateNewsManager.GetPrivateNews();


        }



    }
}
