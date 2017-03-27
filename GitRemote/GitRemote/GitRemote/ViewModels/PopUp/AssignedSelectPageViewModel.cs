using GitRemote.GitHub.Managers;
using GitRemote.Models;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels.PopUp
{
    public class AssignedSelectPageViewModel : BindableBase
    {
        public DelegateCommand ListItemTapped { get; }
        public DelegateCommand ClearButtonTapped { get; }
        public DelegateCommand CancelButtonTapped { get; }
        public ObservableCollection<AssigneeModel> Assignees { get; set; }
        private AssigneeModel _tappedItem;
        public AssigneeModel TappedItem
        {
            get { return _tappedItem; }
            set
            {
                SetProperty(ref _tappedItem, value);
            }
        }

        private FilterManager _manager;

        public AssignedSelectPageViewModel()
        {
            ListItemTapped = new DelegateCommand(OnListItemTapped);
            CancelButtonTapped = new DelegateCommand(OnCancelButtonTapped);
            ClearButtonTapped = new DelegateCommand(OnClearButtonTapped);

            MessagingCenter.Subscribe<FilterManager>(this, SendManagerToFilterPopUp, OnSendManager);
        }

        private async void OnSendManager(FilterManager manager)
        {
            _manager = manager;
            var assignees = await _manager.GetAssigneesAsync();
            Assignees = new ObservableCollection<AssigneeModel>();

            foreach ( var assignee in assignees )
            {
                var model = new AssigneeModel
                {
                    Name = assignee.Name ?? assignee.Login,
                    AvatarUrl = assignee.AvatarUrl
                };

                if ( manager.AssignedName == model.Name ) model.IsActivated = true;

                Assignees.Add(model);
            }

            OnPropertyChanged(nameof(Assignees));
        }

        private async void OnClearButtonTapped()
        {
            MessagingCenter.Unsubscribe<FilterManager>(this, SendManagerToFilterPopUp);
            _manager.AssignedName = "Anyone";
            MessagingCenter.Send(_manager.AssignedName, TakeAssigneeNameFromPopUpPage);
            await PopupNavigation.PopAsync();
        }

        private async void OnCancelButtonTapped()
        {
            MessagingCenter.Unsubscribe<FilterManager>(this, SendManagerToFilterPopUp);
            await PopupNavigation.PopAsync();
        }

        private async void OnListItemTapped()
        {
            MessagingCenter.Unsubscribe<FilterManager>(this, SendManagerToFilterPopUp);
            _manager.AssignedName = TappedItem.Name;
            MessagingCenter.Send(_manager.AssignedName, TakeAssigneeNameFromPopUpPage);
            await PopupNavigation.PopAsync();
        }
    }
}
