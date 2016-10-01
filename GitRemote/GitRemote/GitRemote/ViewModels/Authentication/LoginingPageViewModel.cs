using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
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
        private readonly ISecuredDataProvider _securedDataProvider;
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
            _securedDataProvider = securedDataProvider;
            _checkBox = new ShowPasswordCheckBoxModel();
            _entries = new LogInPageEntriesModel();

            _accountManager = new AccountManager(new ClientAuthorization(_navigationService), _securedDataProvider);

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
            var token = await _accountManager.GetTokenAsync(LoginEntryText, PasswordEntryText);

            _keyboardHelper.HideKeyboard();

            if ( token == "2FA" ) return; // If account has 2FA then I working with it and do below job later

            _accountManager.AddAccount(LoginEntryText, token);

            UserManager.SetLastUser(LoginEntryText);

            var parameters = new NavigationParameters { { "Token", token }, { "Login", LoginEntryText } };

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
