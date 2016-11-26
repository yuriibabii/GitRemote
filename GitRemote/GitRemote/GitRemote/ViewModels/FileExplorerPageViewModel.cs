using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class FileExplorerPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        public NotifyTask SetCurrentBranchTask;
        public NotifyTask SetTreeTask;
        public ObservableCollection<FileExplorerModel> FileTree { get; set; }
        public DelegateCommand ListItemTappedCommand { get; }
        public FileExplorerModel LastTappedItem { get; set; }
        private readonly FileTreeManager _manager;

        public FileExplorerPageViewModel(INavigationService navigationService)
        {
            _manager = new FileTreeManager("UniorDev", "GitRemote");
            SetCurrentBranchTask = NotifyTask.Create(_manager.SetCurrentBranchAsync());
            SetCurrentBranchTask.TaskCompleted.ContinueWith(branchTask =>
            {
                SetTreeTask = NotifyTask.Create(_manager.SetTreeAsync());
                SetTreeTask.TaskCompleted.ContinueWith(treeTask =>
                {
                    FileTree = _manager.GetFiles("GitRemote/");
                    OnPropertyChanged(nameof(FileTree));
                });
            });

            _navigationService = navigationService;
            ListItemTappedCommand = new DelegateCommand(OnListItemTapped);

            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.HardwareBackPressed, OnHardwareBackPressed);
        }

        private void OnListItemTapped()
        {
            if ( LastTappedItem.IsFolder )
            {
                FileTree = _manager.GetFiles(LastTappedItem.Name + "/");
                OnPropertyChanged(nameof(FileTree));
            }
        }

        private void OnHardwareBackPressed(string sender)
        {
            //if ( _manager.PopUpExplorer() )
            //{
            //    Files = NotifyTask.Create(GetFilesAsync());
            //    OnPropertyChanged(nameof(Files));
            //}
            //else
            //    MessagingCenter.Send(this, ConstantsService.Messages.PressHardwareBack);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<object>(this, ConstantsService.Messages.HardwareBackPressed);
        }
    }
}
