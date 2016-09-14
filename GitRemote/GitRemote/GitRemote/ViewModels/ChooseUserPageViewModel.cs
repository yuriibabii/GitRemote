using GitRemote.GitHub;
using GitRemote.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace GitRemote.ViewModels
{
    public class ChooseUserPageViewModel : BindableBase
    {
        public ObservableCollection<string> Users => _userManager?.GetAllUsers();

        public DelegateCommand ListItemTappedCommand { get; }
        private readonly INavigationService _navigationService;
        private readonly ISecuredDataProvider _securedDataProvider;
        private readonly UserManager _userManager;

        public ChooseUserPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            _securedDataProvider = securedDataProvider;
            _userManager = new UserManager(_securedDataProvider);
            ListItemTappedCommand = new DelegateCommand(OnListItemTapped);
        }

        public void OnListItemTapped()
        {

        }
    }
}
