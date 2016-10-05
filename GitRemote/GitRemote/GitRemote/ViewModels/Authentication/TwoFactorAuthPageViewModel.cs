using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Octokit;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;

namespace GitRemote.ViewModels.Authentication
{
    public class TwoFactorAuthPageViewModel : BindableBase, INavigationAware
    {
        private AccountManager _accountManager;
        private readonly INavigationService _navigationService;
        private readonly ISecuredDataProvider _securedDataProvider;
        private readonly IKeyboardHelper _keyboardHelper;
        private GitHubClient _client;
        private readonly IDevice _device;
        private readonly CodePageEntryModel _codePageEntryModel;
        public DelegateCommand HyperLinkTappedCommand { get; }
        public DelegateCommand LogInCommand { get; }

        public string AuthCodeEntryText
        {
            get { return _codePageEntryModel.CodeText; }
            set
            {
                _codePageEntryModel.CodeText = value;
                LogInCommand?.RaiseCanExecuteChanged();
            }
        }

        public TwoFactorAuthPageViewModel(IDevice device, INavigationService navigationService,
            ISecuredDataProvider securedDataProvider, IKeyboardHelper keyboardHelper)
        {
            _keyboardHelper = keyboardHelper;
            _device = device;
            _navigationService = navigationService;
            _securedDataProvider = securedDataProvider;
            _codePageEntryModel = new CodePageEntryModel();
            HyperLinkTappedCommand = new DelegateCommand(OnHyperLinkTapped);
            LogInCommand = new DelegateCommand(OnLogInTapped, () => AuthCodeEntryText?.Length == 6);
        }

        private async void OnHyperLinkTapped()
        {
            await _device.LaunchUriAsync(new Uri(ConstantsService.TwoFactorAuthUrl));
        }

        public async void OnLogInTapped()
        {
            _accountManager = new AccountManager(new ClientAuthorization(), _securedDataProvider);
            var token = await _accountManager.GetTokenAsync(_client, AuthCodeEntryText);
            _keyboardHelper.HideKeyboard();
            _accountManager.AddAccount(_client.Credentials.Login, token);

            UserManager.SetLastUser(_client.Credentials.Login);

            var parameters = new NavigationParameters { { "Session", new Session(_client.Credentials.Login, token) } };

            var navigationStack = new Uri("https://Necessary/" + $"{nameof(ProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}",
                    UriKind.Absolute);

            await _navigationService.NavigateAsync(navigationStack, parameters, animated: false);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( parameters.ContainsKey("Client") )
                _client = ( GitHubClient )parameters["Client"];
        }

        public void OnNavigatedFrom(NavigationParameters parameters) { }
    }
}
