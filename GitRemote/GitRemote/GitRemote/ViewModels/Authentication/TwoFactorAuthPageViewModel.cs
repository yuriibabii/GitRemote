using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
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
        private readonly INavigationService _navigationService;
        private readonly ISecuredDataProvider _securedDataProvider;
        private readonly IKeyboardHelper _keyboardHelper;
        private AccountManager _accountManager;
        private PrivateNewsManager _feedsManager;
        private GitHubClient _gitHubClient;
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
            _feedsManager = new PrivateNewsManager();

            var token = await _accountManager.GetTokenAsync(_gitHubClient, AuthCodeEntryText);
            var privateFeedUlr = await _feedsManager.GetPrivateFeedUrlFromApiAsync(_gitHubClient);

            _keyboardHelper.HideKeyboard();

            _accountManager.AddAccount(_gitHubClient.Credentials.Login, token, privateFeedUlr);

            UserManager.SetLastUser(_gitHubClient.Credentials.Login);

            var parameters = new NavigationParameters { { "Session", new Session(_gitHubClient.Credentials.Login, token, privateFeedUlr) } };

            var navigationStack = new Uri("https://Necessary/" + $"{nameof(PrivateProfilePage)}/{nameof(NavigationBarPage)}/{nameof(DetailPage)}",
                    UriKind.Absolute);

            await _navigationService.NavigateAsync(navigationStack, parameters, animated: false);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( parameters.ContainsKey("Client") )
                _gitHubClient = ( GitHubClient )parameters["Client"];
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters) { }
    }
}
