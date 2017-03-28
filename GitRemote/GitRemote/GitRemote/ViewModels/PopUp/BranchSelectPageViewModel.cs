using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;

namespace GitRemote.ViewModels.PopUp
{
    public class SelectBranchPageViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand ListItemTapped { get; }
        public DelegateCommand CancelButtonTapped { get; }
        public ObservableCollection<BranchSelectModel> Items { get; set; }
        private BranchSelectModel _tappedItem;
        public BranchSelectModel TappedItem
        {
            get { return _tappedItem; }
            set
            {
                SetProperty(ref _tappedItem, value);
            }
        }

        public SelectBranchPageViewModel()
        {
            Debug.WriteLine("IN VIEW MODEL");
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

            Items = new ObservableCollection<BranchSelectModel>();
            var index = 0;
            var counter = 0;

            foreach (var branch in branchesTask.Result)
            {
                var model = new BranchSelectModel { Name = branch.Name, Type = "Branch", IsActivated = false };
                if (model.Name == commitsManager.CurrentBranch)
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            foreach (var tag in tagsTask.Result)
            {
                var model = new BranchSelectModel { Name = tag.Name, Type = "Tag", IsActivated = false };
                if (model.Name == commitsManager.CurrentBranch)
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            RaisePropertyChanged(nameof(Items));
            MessagingCenter.Send(index.ToString(), MessageService.Messages.ScrollToActivatedBranchItem);
        }

        private async void OnSendManager(FileExplorerManager fileExplorerManager)
        {
            var branchesTask = fileExplorerManager.GetBranchesAsync();
            var tagsTask = fileExplorerManager.GetTagsNamesAsync();
            await Task.WhenAll(branchesTask, tagsTask);

            Items = new ObservableCollection<BranchSelectModel>();
            var index = 0;
            var counter = 0;

            foreach (var branch in branchesTask.Result)
            {
                var model = new BranchSelectModel { Name = branch, Type = "Branch", IsActivated = false };
                if (model.Name == fileExplorerManager.CurrentBranch)
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            foreach (var tag in tagsTask.Result)
            {
                var model = new BranchSelectModel { Name = tag, Type = "Tag", IsActivated = false };
                if (model.Name == fileExplorerManager.CurrentBranch)
                {
                    index = counter;
                    model.IsActivated = true;
                }
                Items.Add(model);
                counter++;
            }

            RaisePropertyChanged(nameof(Items));
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

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            Debug.WriteLine("FROM");
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Debug.WriteLine("NAVIGATED");
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Debug.WriteLine("NAVIGATING");
        }
    }
}
