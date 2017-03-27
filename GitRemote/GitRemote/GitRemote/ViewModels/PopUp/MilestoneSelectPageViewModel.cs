using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.ViewModels.PopUp
{
    public class MilestoneSelectPageViewModel : BindableBase
    {
        public DelegateCommand ListItemTapped { get; }
        public DelegateCommand ClearButtonTapped { get; }
        public DelegateCommand CancelButtonTapped { get; }
        public ObservableCollection<MilestoneModel> Milestones { get; set; }
        private MilestoneModel _tappedItem;
        public MilestoneModel TappedItem
        {
            get { return _tappedItem; }
            set
            {
                SetProperty(ref _tappedItem, value);
            }
        }

        private FilterManager _manager;

        public MilestoneSelectPageViewModel()
        {
            ListItemTapped = new DelegateCommand(OnListItemTapped);
            CancelButtonTapped = new DelegateCommand(OnCancelButtonTapped);
            ClearButtonTapped = new DelegateCommand(OnClearButtonTapped);

            MessagingCenter.Subscribe<FilterManager>(this, SendManagerToFilterPopUp, OnSendManager);
        }

        private async void OnSendManager(FilterManager manager)
        {
            _manager = manager;
            var milestones = await _manager.GetMilestonesAsync();
            Milestones = new ObservableCollection<MilestoneModel>();

            foreach ( var milestone in milestones )
            {
                var model = new MilestoneModel()
                {
                    Title = milestone.Title,
                    Description = milestone.Description,
                    IsDescription = StringService.CheckForNullOrEmpty(milestone.Description)
                };

                if ( manager.MilestoneName == model.Title ) model.IsActivated = true;

                Milestones.Add(model);
            }

            OnPropertyChanged(nameof(Milestones));
        }

        private async void OnClearButtonTapped()
        {
            MessagingCenter.Unsubscribe<FilterManager>(this, SendManagerToFilterPopUp);
            _manager.MilestoneName = "None";
            MessagingCenter.Send(_manager.MilestoneName, TakeMilestoneNameFromPopUpPage);
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
            _manager.MilestoneName = TappedItem.Title;
            MessagingCenter.Send(_manager.MilestoneName, TakeMilestoneNameFromPopUpPage);
            await PopupNavigation.PopAsync();
        }
    }
}
