using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class SelectBranchPopUpPageViewModel : BindableBase
    {
        public DelegateCommand ListItemTapped { get; }
        public DelegateCommand CancelButtonTapped { get; }
        public ObservableCollection<SelectBranchPopUpModel> Items { get; set; }
        private SelectBranchPopUpModel _tappedItem;
        public SelectBranchPopUpModel TappedItem
        {
            get { return _tappedItem; }
            set
            {
                SetProperty(ref _tappedItem, value);
            }
        }

        public SelectBranchPopUpPageViewModel()
        {
            ListItemTapped = new DelegateCommand(OnListItemTapped);
            CancelButtonTapped = new DelegateCommand(OnCancelButtonTapped);
            MessagingCenter.Subscribe<FileExplorerManager>(this, ConstantsService.Messages.SendManagerToBranchPopUpPage, OnSendManager);
        }

        private async void OnSendManager(FileExplorerManager fileExplorerManager)
        {
            var branchesTask = fileExplorerManager.GetBranchesAsync();
            await branchesTask.ContinueWith(task =>
            {
                var tagsTask = fileExplorerManager.GetTagsNamesAsync();
                tagsTask.ContinueWith(t =>
                {
                    Items = new ObservableCollection<SelectBranchPopUpModel>();

                    foreach ( var branch in branchesTask.Result )
                    {
                        var model = new SelectBranchPopUpModel { Name = branch, Type = "Branch", IsActivated = false };
                        if ( model.Name == fileExplorerManager.CurrentBranch )
                            model.IsActivated = true;
                        Items.Add(model);
                    }

                    foreach ( var tag in tagsTask.Result )
                    {
                        var model = new SelectBranchPopUpModel { Name = tag, Type = "Tag", IsActivated = false };
                        if ( model.Name == fileExplorerManager.CurrentBranch )
                            model.IsActivated = true;
                        Items.Add(model);
                    }

                    OnPropertyChanged(nameof(Items));
                    MessagingCenter.Send(fileExplorerManager.CurrentBranch, ConstantsService.Messages.ScrollToActivatedBranchItem);
                });
            });


        }

        private async void OnCancelButtonTapped()
        {
            MessagingCenter.Unsubscribe<FileExplorerManager>(this, ConstantsService.Messages.SendManagerToBranchPopUpPage);
            await PopupNavigation.PopAsync();
        }

        private async void OnListItemTapped()
        {
            MessagingCenter.Send(TappedItem, ConstantsService.Messages.TakeBranchModelFromPopUpPage);
            await PopupNavigation.PopAsync();
        }
    }
}
