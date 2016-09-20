using GitRemote.DI;
using GitRemote.GitHub;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class ChooseUserPageViewModel : BindableBase
    {
        public ObservableCollection<string> Users => _userManager?.GetAllUsers();
        private ViewCell _previousCell, _currentCell;
        public DelegateCommand<ViewCell> ListItemTappedCommand { get; }
        private readonly INavigationService _navigationService;
        private readonly ISecuredDataProvider _securedDataProvider;
        private readonly UserManager _userManager;

        public ChooseUserPageViewModel(INavigationService navigationService, ISecuredDataProvider securedDataProvider)
        {
            _navigationService = navigationService;
            _securedDataProvider = securedDataProvider;
            _userManager = new UserManager(_securedDataProvider);
            ListItemTappedCommand = new DelegateCommand<ViewCell>(OnListItemTapped);
        }

        public void OnListItemTapped(ViewCell cell)
        {
            _currentCell = cell;

            ( ( ListView )_currentCell.Parent ).SelectedItem = 0;

            if ( _previousCell != null )
                _previousCell.View.BackgroundColor = Color.Default;

            _currentCell.View.BackgroundColor = Color.FromHex("#D8A6C9");

            _previousCell = _currentCell;
        }
    }
}
