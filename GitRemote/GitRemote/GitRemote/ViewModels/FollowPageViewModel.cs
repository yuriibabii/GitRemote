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
using GitRemote.Common;
using GitRemote.GitHub.Managers;
using GitRemote.Views;
using MvvmHelpers;
using Prism.Commands;
using Xamarin.Forms;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.ViewModels
{
    public class FollowPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public NotifyTask<ObservableRangeCollection<FollowModel>> Follows { get; set; }
        private readonly FollowManager _manager;
        private int _pageNumber = 1;

        #region Commands

        public DelegateCommand ItemTappedCommand => new DelegateCommand(OnItemTapped);
        public DelegateCommand<object> LoadMoreCommand => new DelegateCommand<object>(OnLoadMore, CanLoadMore);
        public DelegateCommand RefreshCommand => new DelegateCommand(OnRefresh, CanRefresh);

        #endregion

        #region Properties

        public FollowModel TappedItem { get; set; }

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

        public FollowPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;

            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());

            var session = new Session(UserManager.GetLastUser(), store.Properties.First().Value,
                store.Properties[PrivateFeedUrl]);

            var navigationParameters = new NavigationParameters { { nameof(Session), session } };

            _manager = new FollowManager(session);

            Follows = NotifyTask.Create(_manager.GetFollowsAsync());
        }

        private void OnLoadMore(object model)
        {
            _pageNumber++;
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                var newItems = await _manager.GetFollowsAsync(_pageNumber);
                Follows.Result.AddRange(newItems);
                IsBusy = false;
            });
        }

        private void OnRefresh()
        {
            _pageNumber = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                Follows = NotifyTask.Create(_manager.GetFollowsAsync());
                var loadTask = Follows.TaskCompleted;
                loadTask.ContinueWith(task =>
                {
                    RaisePropertyChanged(nameof(Follows));
                    IsBusy = false;
                });
            });
        }

        private bool CanLoadMore(object model)
        {
            if (IsBusy) return false;
            if (Follows.Result.Count < 1) return false;

            var follow = (FollowModel)model;
            var lastItem = Follows.Result[Follows.Result.Count - 1];
            return lastItem.Name == follow.Name;
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnItemTapped()
        {
            //TODO
        }
    }
}
