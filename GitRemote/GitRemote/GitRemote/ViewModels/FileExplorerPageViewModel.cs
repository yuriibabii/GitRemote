using GitRemote.GitHub.Managers;
using GitRemote.Models;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GitRemote.ViewModels
{
    public class FileExplorerPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;
        public NotifyTask<ObservableCollection<FileExplorerModel>> Files { get; }
        private readonly FileExplorerManager _manager;

        public FileExplorerPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _manager = new FileExplorerManager("UniorDev", "GitRemote");
            Files = NotifyTask.Create(GetFilesAsync());
        }

        private async Task<ObservableCollection<FileExplorerModel>> GetFilesAsync()
        {
            return new ObservableCollection<FileExplorerModel>(await _manager.GetFilesAsync());
        }


    }
}
