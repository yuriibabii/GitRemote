using GitRemote.GitHub.Managers;
using GitRemote.Models;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class FileExplorerPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<FileExplorerModel>> Files { get; }
        private readonly FileExplorerManager _manager;
        //public DelegateCommand<Label> ListItemTappedCommand { get; }

        public FileExplorerPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            //ListItemTappedCommand = new DelegateCommand<Label>(OnListItemTapped);
            _manager = new FileExplorerManager("UniorDev", "GitRemote");
            Files = NotifyTask.Create(GetFilesAsync());
        }

        private void OnListItemTapped(Label label)
        {
            //Files = NotifyTask.Create(GetFilesAsync(Name))
        }

        private async Task<ObservableCollection<FileExplorerModel>> GetFilesAsync()
        {
            return new ObservableCollection<FileExplorerModel>(await _manager.GetFilesAsync());
        }


    }
}
