using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class PublicNewsPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public NotifyTask<ObservableCollection<PublicNewsModel>> PublicNews { get; }
        private readonly PublicNewsManager _publicNewsManager;
        public GridLength ColumnWidth { get; set; }

        public PublicNewsPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            _publicNewsManager = new PublicNewsManager();

            //PublicNews = NotifyTask.Create(GetPublicNewsAsync());

            // It does to fit title to display width
            ColumnWidth = new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
                ? App.ScreenWidth - ConstantsService.OtherWidth
                : ConstantsService.MaxNormalWidthForTitle);
        }

        //private async Task<ObservableCollection<PublicNewsModel>> GetPublicNewsAsync()
        //{
        //    return new ObservableCollection<PublicNewsModel>
        //        (await _publicNewsManager.GetPublicNews());
        //}
    }
}
