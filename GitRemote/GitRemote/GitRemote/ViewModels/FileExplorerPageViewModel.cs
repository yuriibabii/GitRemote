using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class FileExplorerPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<FileExplorerModel>> Files { get; set; }
        private readonly FileExplorerManager _manager;
        public DelegateCommand<Label> ListItemTappedCommand { get; }

        public FileExplorerPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ListItemTappedCommand = new DelegateCommand<Label>(OnListItemTapped);
            _manager = new FileExplorerManager("UniorDev", "GitRemote");
            Files = NotifyTask.Create(GetFilesAsync());
            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.HardwareBackPressed, OnHardwareBackPressed);
        }

        private void OnListItemTapped(Label label)
        {
            Files = NotifyTask.Create(GetFilesAsync(label.Text + "/"));
            OnPropertyChanged(nameof(Files));
        }

        private async Task<ObservableCollection<FileExplorerModel>> GetFilesAsync(string path = "")
        {
            return new ObservableCollection<FileExplorerModel>(await _manager.GetFilesAsync(path));
        }

        private void OnHardwareBackPressed(string sender)
        {
            if ( _manager.PopUpExplorer() )
            {
                Files = NotifyTask.Create(GetFilesAsync());
                OnPropertyChanged(nameof(Files));
            }
            else
                MessagingCenter.Send(this, ConstantsService.Messages.PressHardwareBack);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<object>(this, ConstantsService.Messages.HardwareBackPressed);
        }
    }
}
