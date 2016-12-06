using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
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

            MessagingCenter.Subscribe<FileExplorerManager>(this,
                MessageService.Messages.SendManagerToBranchPopUpPage, OnSendManager);

            MessagingCenter.Subscribe<CommitsManager>(this,
                MessageService.Messages.SendManagerToBranchPopUpPage, OnSendManager);
        }


        private async void OnSendManager(CommitsManager commitsManager)
        {
            var branchesTask = commitsManager.GetBranchesAsync();
            var tagsTask = commitsManager.GetTagsAsync();
            await Task.WhenAll(branchesTask, tagsTask);

            Items = new ObservableCollection<SelectBranchPopUpModel>();
            var index = 0;
            var counter = 0;

            foreach ( var branch in branchesTask.Result )
            {
                var model = new SelectBranchPopUpModel { Name = branch.Name, Type = "Branch", IsActivated = false };
                if ( model.Name == commitsManager.CurrentBranch )
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            foreach ( var tag in tagsTask.Result )
            {
                var model = new SelectBranchPopUpModel { Name = tag.Name, Type = "Tag", IsActivated = false };
                if ( model.Name == commitsManager.CurrentBranch )
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            OnPropertyChanged(nameof(Items));
            MessagingCenter.Send(index.ToString(), MessageService.Messages.ScrollToActivatedBranchItem);
        }

        private async void OnSendManager(FileExplorerManager fileExplorerManager)
        {
            var branchesTask = fileExplorerManager.GetBranchesAsync();
            var tagsTask = fileExplorerManager.GetTagsNamesAsync();
            await Task.WhenAll(branchesTask, tagsTask);

            Items = new ObservableCollection<SelectBranchPopUpModel>();
            var index = 0;
            var counter = 0;

            foreach ( var branch in branchesTask.Result )
            {
                var model = new SelectBranchPopUpModel { Name = branch, Type = "Branch", IsActivated = false };
                if ( model.Name == fileExplorerManager.CurrentBranch )
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            foreach ( var tag in tagsTask.Result )
            {
                var model = new SelectBranchPopUpModel { Name = tag, Type = "Tag", IsActivated = false };
                if ( model.Name == fileExplorerManager.CurrentBranch )
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            OnPropertyChanged(nameof(Items));
            MessagingCenter.Send(index.ToString(), MessageService.Messages.ScrollToActivatedBranchItem);
        }

        private async void OnCancelButtonTapped()
        {
            MessagingCenter.Unsubscribe<CommitsManager>(this, MessageService.Messages.SendManagerToBranchPopUpPage);
            MessagingCenter.Unsubscribe<FileExplorerManager>(this, MessageService.Messages.SendManagerToBranchPopUpPage);
            await PopupNavigation.PopAsync();
        }

        private async void OnListItemTapped()
        {
            MessagingCenter.Unsubscribe<CommitsManager>(this, MessageService.Messages.SendManagerToBranchPopUpPage);
            MessagingCenter.Unsubscribe<FileExplorerManager>(this, MessageService.Messages.SendManagerToBranchPopUpPage);
            MessagingCenter.Send(TappedItem, MessageService.Messages.TakeBranchModelFromPopUpPage);
            await PopupNavigation.PopAsync();
        }
    }
}
