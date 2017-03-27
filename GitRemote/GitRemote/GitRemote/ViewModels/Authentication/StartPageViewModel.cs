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
        public DelegateCommand LogInWithExistUserButtonCommand => new DelegateCommand(OnLogInWithExistUserButtonTapped);
        public DelegateCommand CreateNewUserButtonCommand => new DelegateCommand(OnCreateNewUserButtonTapped);
        private readonly INavigationService _navigationService;

        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private void OnLogInWithExistUserButtonTapped()
        {
            _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(ChooseUserPage)}", animated: false);
        }

        private void OnCreateNewUserButtonTapped()
        {
            _navigationService.NavigateAsync($"{nameof(NavigationBarPage)}/{nameof(LoginingPage)}", animated: false);
        }
    }
}
