using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace GitRemote.ViewModels
{
    public class LoginingPageViewModel : BindableBase
    {
        private readonly ShowPasswordCheckBoxModel _checkBox;
        private readonly LogInPageEntriesModel _entries;
        private readonly INavigationService _navigationService;
        private readonly IKeyboardHelper _keyboardHelper;
        private readonly ISecuredDataProvider _securedDataProvider;
        private readonly AccountManager _accountManager;
        public DelegateCommand CheckedCommand { get; }
        public DelegateCommand LogInCommand { get; }

        public bool IsUnChecked => _checkBox.IsUnChecked;
        public string CheckBoxImagePath => _checkBox.ImageSource;

        public string LoginEntryText
        {
            get { return _entries.LoginText; }
            set
            {
                _entries.LoginText = value;
                LogInCommand.RaiseCanExecuteChanged();
            }
        }

        public string PasswordEntryText
        {
            get { return _entries.PasswordText; }
            set
            {
                _entries.PasswordText = value;
                LogInCommand.RaiseCanExecuteChanged();
            }
        }

        public LoginingPageViewModel(INavigationService navigationService, IKeyboardHelper keyboardHelper,
            ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            _keyboardHelper = keyboardHelper;
            _securedDataProvider = securedDataProvider;
            _checkBox = new ShowPasswordCheckBoxModel();
            _entries = new LogInPageEntriesModel();

            _accountManager = new AccountManager(new ClientAuthorization(), _securedDataProvider);

            Func<bool> isLogInCommandEnable = () =>
                StringService.CheckForNullOrEmpty(_entries.LoginText, _entries.PasswordText);

            CheckedCommand = new DelegateCommand(OnCheckBoxTapped);
            LogInCommand = new DelegateCommand(OnLogInTapped, isLogInCommandEnable);
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

        public void OnLogInTapped()
        {
            _keyboardHelper.HideKeyboard();
            _accountManager.AddAccount(LoginEntryText, PasswordEntryText);

            var navigationStack = new Uri("https://Necessary/" + $"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}",
                UriKind.Absolute);

            _navigationService.NavigateAsync(navigationStack, animated: false);
        }
    }
}
