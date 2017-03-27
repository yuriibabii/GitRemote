using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Views.PopUp;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels
{
    public class FilterPageViewModel : BindableBase, INavigationAware
    {
        private const string CheckedRadioButtonImage = "ic_radio_button_on_24px.png";
        private const string UnCheckedRadioButtonImage = "ic_radio_button_unchecked_black_24dp.png";

        private FilterManager _manager;

        #region Props

        private string _type = string.Empty;
        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _title = "Filter";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isOpenStatus = true;

        private string _openStatusImage = CheckedRadioButtonImage;
        public string OpenStatusImage
        {
            get { return _openStatusImage; }
            set { SetProperty(ref _openStatusImage, value); }
        }

        private string _closedStatusImage = UnCheckedRadioButtonImage;
        public string ClosedStatusImage
        {
            get { return _closedStatusImage; }
            set { SetProperty(ref _closedStatusImage, value); }
        }

        private string _assignedName = "Anyone";
        public string AssignedName
        {
            get { return _assignedName; }
            set { SetProperty(ref _assignedName, value); }
        }

        private string _milestoneName = "None";
        public string MilestoneName
        {
            get { return _milestoneName; }
            set { SetProperty(ref _milestoneName, value); }
        }
        #endregion

        #region Commands

        public DelegateCommand OpenStatusTapped { get; }
        public DelegateCommand ClosedStatusTapped { get; }
        public DelegateCommand OpenAssignTapped { get; }
        public DelegateCommand OpenMilestoneTapped { get; }

        #endregion

        public FilterPageViewModel()
        {
            OpenStatusTapped = new DelegateCommand(OnOpenStatusTapped);
            ClosedStatusTapped = new DelegateCommand(OnClosedStatusTapped);
            OpenAssignTapped = new DelegateCommand(OnOpenAssignedTapped);
            OpenMilestoneTapped = new DelegateCommand(OnOpenMilestoneTapped);
        }

        private void OnAssigneeReceived(string assigneeName)
        {
            MessagingCenter.Unsubscribe<string>(this, TakeAssigneeNameFromPopUpPage);
            AssignedName = assigneeName;
        }

        private void OnMilestoneReceived(string milestoneName)
        {
            MessagingCenter.Unsubscribe<string>(this, TakeMilestoneNameFromPopUpPage);
            MilestoneName = milestoneName;
        }

        private void OnOpenStatusTapped()
        {
            if ( _isOpenStatus ) return;

            _isOpenStatus = !_isOpenStatus;
            OpenStatusImage = CheckedRadioButtonImage;
            ClosedStatusImage = UnCheckedRadioButtonImage;
        }

        private void OnClosedStatusTapped()
        {
            if ( !_isOpenStatus ) return;

            _isOpenStatus = !_isOpenStatus;
            ClosedStatusImage = CheckedRadioButtonImage;
            OpenStatusImage = UnCheckedRadioButtonImage;
        }

        private void OnOpenAssignedTapped()
        {
            MessagingCenter.Subscribe<string>(this, TakeAssigneeNameFromPopUpPage, OnAssigneeReceived);
            PopupNavigation.PushAsync(new AssignedSelectPage());
            MessagingCenter.Send(_manager, SendManagerToFilterPopUp);
        }

        private void OnOpenMilestoneTapped()
        {
            MessagingCenter.Subscribe<string>(this, TakeMilestoneNameFromPopUpPage, OnMilestoneReceived);
            PopupNavigation.PushAsync(new MilestoneSelectPage());
            MessagingCenter.Send(_manager, SendManagerToFilterPopUp);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey("Session") ||
                !parameters.ContainsKey("OwnerName") ||
                !parameters.ContainsKey("ReposName") ||
                !parameters.ContainsKey("Type") )
                return;

            var session = parameters["Session"] as Session;
            var ownerName = parameters["OwnerName"] as string;
            var reposName = parameters["ReposName"] as string;
            Type = parameters["Type"] as string;
            Title = "Filter " + Type;
            _manager = new FilterManager(session, ownerName, reposName);
        }
    }
}
