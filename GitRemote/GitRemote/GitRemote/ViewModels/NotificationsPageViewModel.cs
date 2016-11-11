using GitRemote.GitHub;
using GitRemote.Models;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using GitRemote.GitHub.Managers;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class NotificationsPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        private Session _session;
        private ObservableCollection<NotificationModel> _notifications;

        public ObservableCollection<NotificationModel> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value); }
        }

        private NotificationsManager _notificationsManager;

        private const int MaxNormalWidthForTitle = 450;
        private const int OtherWidth = 95;
        public GridLength ColumnWidth { get; set; }

        public NotificationsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < MaxNormalWidthForTitle
                ? App.ScreenWidth - OtherWidth
                : MaxNormalWidthForTitle);
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey("Session") ) return;

            _session = parameters["Session"] as Session;

            _notificationsManager = new NotificationsManager(_session);

            Notifications = new ObservableCollection<NotificationModel>
                (await _notificationsManager.GetNotificationsAsync());
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }
    }
}
