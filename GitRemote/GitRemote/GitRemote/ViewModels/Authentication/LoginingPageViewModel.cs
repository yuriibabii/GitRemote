using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Octokit;
using Octokit.Internal;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace GitRemote.ViewModels.Authentication
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly ShowPasswordCheckBoxModel _checkBox;
        private readonly LogInPageEntriesModel _entries;
        private readonly INavigationService _navigationService;
        private readonly IKeyboardHelper _keyboardHelper;
        private readonly AccountManager _accountManager;
        private readonly IDevice _device;
        public DelegateCommand CheckedCommand { get; }
        public DelegateCommand LogInCommand { get; }
        public DelegateCommand HyperLinkTappedCommand { get; }

        public bool IsUnChecked => _checkBox.IsUnChecked;
        public string CheckBoxImagePath => _checkBox.ImageSource;

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
            _checkBox = new ShowPasswordCheckBoxModel();
            _entries = new LogInPageEntriesModel();

            _accountManager = new AccountManager(new ClientAuthorization(_navigationService), securedDataProvider);


            Func<bool> isLogInCommandEnable = () =>
                StringService.CheckForNullOrEmpty(_entries.LoginText, _entries.PasswordText);


            CheckedCommand = new DelegateCommand(OnCheckBoxTapped);
            LogInCommand = new DelegateCommand(OnLogInTapped, isLogInCommandEnable);
            HyperLinkTappedCommand = new DelegateCommand(OnHyperLinkTapped);
            //_keyboardHelper.ShowKeyboard();
        }

        public void OnCheckBoxTapped()
        {
            _keyboardHelper.ShowKeyboard();
            _checkBox.ChangeCheckedProperty();
            _checkBox.ChangeImageState();
            OnPropertyChanged(nameof(IsUnChecked));
            OnPropertyChanged(nameof(CheckBoxImagePath));
        }

        public async void OnLogInTapped()
        {
            var _newsManager = new PrivateNewsManager();

            var gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                 new InMemoryCredentialStore(new Credentials(LoginEntryText, PasswordEntryText)));

            var token = await _accountManager.GetTokenAsync(gitHubClient);

            var privateFeedUrl = await _newsManager.GetPrivateFeedUrlFromApiAsync(gitHubClient);

            var session = new Session(LoginEntryText, token, privateFeedUrl);

            _keyboardHelper.HideKeyboard();

            if ( token == "2FA" ) return; // If account hasn't 2FA then I working with it and do below job later

            _accountManager.AddAccount(LoginEntryText, token, privateFeedUrl);

            UserManager.SetLastUser(LoginEntryText);

            var parameters = new NavigationParameters { { "Session", session } };

            var navigationStack = new Uri("https://Necessary/" + $"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}",
                    UriKind.Absolute);

            await _navigationService.NavigateAsync(navigationStack, parameters, animated: false);
        }

        private async void OnHyperLinkTapped()
        {
            await _device.LaunchUriAsync(new Uri(ConstantsService.GitHubOfficialPageUrl));
        }
    }
}
