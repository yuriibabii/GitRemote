using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Octokit;
using Octokit.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using Xamarin.Forms;
using static GitRemote.Services.FontIconsService.FontAwesome;

namespace GitRemote.ViewModels.Authentication
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly LogInPageEntriesModel _entries = new LogInPageEntriesModel();
        private readonly INavigationService _navigationService;
        private readonly IKeyboardHelper _keyboardHelper;
        private readonly AccountManager _accountManager;
        private readonly IDevice _device;
        public DelegateCommand CheckedCommand => new DelegateCommand(OnCheckBoxTapped);
        //public DelegateCommand LogInCommand => new DelegateCommand(OnLogInTapped,
        //    () => !StringService.IsNullOrEmpty(_entries.LoginText, _entries.PasswordText));
        //public DelegateCommand LogInCommand { get; } = new DelegateCommand(OnLogInTapped,
        //    () => !StringService.IsNullOrEmpty(_entries.LoginText, _entries.PasswordText));
        public DelegateCommand LogInCommand { get; }
        public DelegateCommand HyperLinkTappedCommand => new DelegateCommand(OnHyperLinkTapped);

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get { return _isPasswordVisible; }
            set
            {
                SetProperty(ref _isPasswordVisible, value);
                RaisePropertyChanged(nameof(CheckBoxIcon));
                RaisePropertyChanged(nameof(CheckBoxColor));
            }
        }

        public string CheckBoxIcon => IsPasswordVisible ? CheckedSquare : UnCheckedSquare;
        public Color CheckBoxColor => IsPasswordVisible ? Color.Green : Color.Black;

        public string LoginEntryText
        {
            get { return _entries.LoginText; }
            set
            {
                _entries.LoginText = value;
                LogInCommand?.RaiseCanExecuteChanged();
            }
        }

        public string PasswordEntryText
        {
            get { return _entries.PasswordText; }
            set
            {
                _entries.PasswordText = value;
                LogInCommand?.RaiseCanExecuteChanged();
            }
        }

        public LoginingPageViewModel(INavigationService navigationService, IKeyboardHelper keyboardHelper,
            ISecuredDataProvider securedDataProvider, IDevice device)
        {
            _device = device;
            _navigationService = navigationService;
            _keyboardHelper = keyboardHelper;
            _accountManager = new AccountManager(new ClientAuthorization(_navigationService), securedDataProvider);
            Func<bool> canExecuteLogIn = () => !StringService.IsNullOrEmpty(_entries.LoginText, _entries.PasswordText);
            LogInCommand = new DelegateCommand(OnLogInTapped, canExecuteLogIn);
        }

        public void OnCheckBoxTapped()
        {
            _keyboardHelper.ShowKeyboard();
            IsPasswordVisible = !IsPasswordVisible;
        }

        public async void OnLogInTapped()
        {
            _keyboardHelper.HideKeyboard();

            var newsManager = new PrivateNewsManager();

            var gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                 new InMemoryCredentialStore(new Credentials(LoginEntryText, PasswordEntryText)));

            var token = await _accountManager.GetTokenAsync(gitHubClient);

            if (token == "2FA") return; // If account hasn't 2FA then I working with it and do below job later

            var privateFeedUrl = await newsManager.GetPrivateFeedUrlFromApiAsync(gitHubClient);

            var session = new Session(LoginEntryText, token, privateFeedUrl);

            _accountManager.AddAccount(LoginEntryText, token, privateFeedUrl);

            UserManager.SetLastUser(LoginEntryText);

            var parameters = new NavigationParameters { { nameof(Session), session } };

            var navigationStack = new Uri("https://Necessary/" +
                $"{nameof(PrivateProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}", UriKind.Absolute);

            await _navigationService.NavigateAsync(navigationStack, parameters, animated: false);
        }

        private void OnHyperLinkTapped()
        {
            _device.LaunchUriAsync(new Uri(ConstantsService.GitHubOfficialPageUrl));
        }
    }
}
