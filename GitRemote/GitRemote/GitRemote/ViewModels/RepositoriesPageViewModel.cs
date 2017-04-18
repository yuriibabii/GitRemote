using System.Collections;
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
using MvvmHelpers;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Common.Enums;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.ViewModels
{
    public class RepositoriesPageViewModel : BindableBase
    {
        private int _pageNumber = 1;
        private readonly Session _session;
        private readonly IEventAggregator _eventAggregator;
        public NotifyTask<ObservableRangeCollection<GroupingModel<string, RepositoryModel>>>
            GroupedRepositories
        { get; set; }

        private readonly RepositoriesManager _manager;

        #region Commands

        public DelegateCommand ItemTappedCommand => new DelegateCommand(OnItemTapped);
        public DelegateCommand<object> LoadMoreCommand => new DelegateCommand<object>(OnLoadMore, CanLoadMore);
        public DelegateCommand RefreshCommand => new DelegateCommand(OnRefresh, CanRefresh);

        #endregion

        #region Properties

        public string StarIcon => FontIconsService.Octicons.Star;
        public string ForkIcon => FontIconsService.Octicons.RepoForked;

        public RepositoryModel TappedItem { get; set; }
        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                var changed = SetProperty(ref _isBusy, value);
                if (changed) RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        public RepositoriesPageViewModel(ISecuredDataProvider securedDataProvider,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            var token = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            _session = new Session(UserManager.GetLastUser(), token.Properties.First().Value);
            _manager = new RepositoriesManager(_session);
            GroupedRepositories = NotifyTask.Create(_manager.GetRepositoriesAsync());
        }

        private void OnLoadMore(object model)
        {
            _pageNumber++;
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                var newItems = await _manager.GetRepositoriesAsync(_pageNumber);
                GroupedRepositories.Result.AddRange(newItems);
                IsBusy = false;
            });
        }

        private void OnRefresh()
        {
            _pageNumber = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                GroupedRepositories = NotifyTask.Create(_manager.GetRepositoriesAsync(_pageNumber));
                var loadTask = GroupedRepositories.TaskCompleted;
                loadTask.ContinueWith(task =>
                {
                    RaisePropertyChanged(nameof(GroupedRepositories));
                    IsBusy = false;
                });
            });
        }

        private bool CanLoadMore(object model)
        {
            if (IsBusy) return false;
            if (GroupedRepositories.Result.Count < 1) return false;

            var list = model as IList;
            if (list == null) return false;

            var repo = (RepositoryModel)list[0];
            var indexOfLastKey = GroupedRepositories.Result.Count - 1;
            var itemsOfKey = GroupedRepositories.Result[indexOfLastKey];
            var lastItemOfLastKey = itemsOfKey[itemsOfKey.Count - 1];

            return lastItemOfLastKey.Name == repo.Name &&
                   lastItemOfLastKey.OwnerName == repo.OwnerName &&
                   lastItemOfLastKey.Type == repo.Type;
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnItemTapped()
        {
            var ownerName = TappedItem.OwnerName;
            var reposName = TappedItem.Name;
            var parameters = new NavigationParameters
            {
                { OwnerName, ownerName},
                { ReposName, reposName},
                { nameof(Session), _session}
            };

            var path = TappedItem.Type == RepositoriesTypes.Fork
                ? $"{nameof(ForkedRepositoryPage)}"
                : $"{nameof(PublicRepositoryPage)}";

            _eventAggregator
                .GetEvent<MessageService.DoNavigation>()
                .Publish(new DoNavigationModel(path, parameters));
        }
    }
}
