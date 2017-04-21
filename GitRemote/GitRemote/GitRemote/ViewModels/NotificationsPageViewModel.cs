using GitRemote.GitHub;
using GitRemote.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using GitRemote.GitHub.Managers;
using MvvmHelpers;
using Nito.Mvvm;
using Prism.Commands;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class NotificationsPageViewModel : BindableBase, INavigationAware
    {
        #region Commands

        public DelegateCommand ItemTappedCommand => new DelegateCommand(OnItemTapped);
        public DelegateCommand<object> LoadMoreCommand => new DelegateCommand<object>(OnLoadMore, CanLoadMore);
        public DelegateCommand RefreshCommand => new DelegateCommand(OnRefresh, CanRefresh);

        #endregion

        #region Properties

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                var changed = SetProperty(ref _isBusy, value);
                if ( changed ) RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        public GridLength ColumnWidth { get; } = new GridLength(App.ScreenWidth < MaxNormalWidthForTitle
                ? App.ScreenWidth - OtherWidth
                : MaxNormalWidthForTitle);

        #endregion

        private INavigationService _navigationService;
        private Session _session;
        public NotifyTask<ObservableRangeCollection<NotificationModel>> Notifications { get; set; }
        private NotificationsManager _manager;
        private int _pageNumber = 1;
        private const int MaxNormalWidthForTitle = 450;
        private const int OtherWidth = 95;

        public NotificationsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

        }

        private void OnLoadMore(object model)
        {
            _pageNumber++;
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                var newItems = await _manager.GetNotificationsAsync(_pageNumber);
                Notifications.Result.AddRange(newItems);
                IsBusy = false;
            });
        }

        private void OnRefresh()
        {
            _pageNumber = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                Notifications = NotifyTask.Create(_manager.GetNotificationsAsync());
                var loadTask = Notifications.TaskCompleted;
                loadTask.ContinueWith(task =>
                {
                    RaisePropertyChanged(nameof(Notifications));
                    IsBusy = false;
                });
            });
        }

        private bool CanLoadMore(object model)
        {
            if ( IsBusy ) return false;

            if ( Notifications.Result.Count < 1 ) return false;

            var news = ( PrivateNewsModel )model;
            return Notifications.Result[Notifications.Result.Count - 1].Title == news.Title;
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnItemTapped()
        {
            //TODO
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey(nameof(Session)) ) return;
            _session = parameters[nameof(Session)] as Session;
            _manager = new NotificationsManager(_session);
            Notifications = NotifyTask.Create(_manager.GetNotificationsAsync());
            var completedTask = Notifications.TaskCompleted;
            completedTask.ContinueWith(task =>
            {
                Device.BeginInvokeOnMainThread(
                    () => RaisePropertyChanged(nameof(Notifications)));
            });

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }
    }
}
