using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Models.PrivateNewsModel;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.ViewModels
{
    public class PrivateNewsPageViewModel : BindableBase
    {
        private readonly Session _session;
        public NotifyTask<ObservableCollection<PrivateNewsModel>> PrivateNews { get; }
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand ItemTappedCommand => new DelegateCommand(OnItemTapped);
        public PrivateNewsModel TappedItem { get; set; }
        public GridLength ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);

        public PrivateNewsPageViewModel(ISecuredDataProvider securedDataProvider, IEventAggregator eventAggregator)
        {
            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            var lastUser = UserManager.GetLastUser();
            var token = store.Properties.First().Value;
            var privateFeedUrl = store.Properties[PrivateFeedUrl];
            _session = new Session(lastUser, token, privateFeedUrl);
            var privateNewsManager = new PrivateNewsManager(_session);
            PrivateNews = NotifyTask.Create(privateNewsManager.GetPrivateNews);
            _eventAggregator = eventAggregator;
        }

        private void OnItemTapped()
        {
            var splited = TappedItem.Target.Split('/');
            var ownerName = TappedItem.ActionType == ActionTypes.Forked
                ? TappedItem.Perfomer
                : splited[0];

            var reposName = splited[1];

            //TODO
            //Not implemented navigation to IssuePage when content of news is Issue.
            //Because of lack of this page.

            var parameters = new NavigationParameters
            {
                { OwnerName, ownerName},
                { ReposName, reposName},
                { nameof(Session), _session},
            };

            var path = $"{nameof(PublicRepositoryPage)}";

            _eventAggregator
                .GetEvent<MessageService.DoNavigation>()
                .Publish(new DoNavigationModel(path, parameters));
        }
    }
}
