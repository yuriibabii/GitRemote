using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ChooseUserPage = GitRemote.Views.Authentication.ChooseUserPage;
using LoginingPage = GitRemote.Views.Authentication.LoginingPage;

namespace GitRemote.ViewModels.Authentication
{
    public class StartPageViewModel : BindableBase
    {
        public string StartPageImagePath => "gitremote_logo.png";
        public DelegateCommand LogInWithExistUserButtonCommand { get; }
        public DelegateCommand CreateNewUserButtonCommand { get; }
        private readonly INavigationService _navigationService;

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LogInWithExistUserButtonCommand = new DelegateCommand(OnLogInWithExistUserButtonTapped);
            CreateNewUserButtonCommand = new DelegateCommand(OnCreateNewUserButtonTapped);
        }

        private async void OnLogInWithExistUserButtonTapped()
        {
            await _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(ChooseUserPage)}");
        }

        private async void OnCreateNewUserButtonTapped()
        {
            await _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(LoginingPage)}");
        }
    }
}
