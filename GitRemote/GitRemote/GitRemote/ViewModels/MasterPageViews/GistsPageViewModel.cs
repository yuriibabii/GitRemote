using GitRemote.DI;
using GitRemote.GitHub;
using Prism.Mvvm;
using Prism.Navigation;

namespace GitRemote.ViewModels.MasterPageViews
{
    public class GistsPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private Session _session;
        private readonly IMetricsHelper _metricsHelper;
        private string _minePageTitle = string.Empty;
        public string MinePageTitle
        {
            get { return _minePageTitle; }
            set { SetProperty(ref _minePageTitle, value); }
        }

        private string _starredPageTitle = string.Empty;
        public string StarredPageTitle
        {
            get { return _starredPageTitle; }
            set { SetProperty(ref _starredPageTitle, value); }
        }

        private string _allPageTitle = string.Empty;
        public string AllPageTitle
        {
            get { return _allPageTitle; }
            set { SetProperty(ref _allPageTitle, value); }
        }

        public GistsPageViewModel(INavigationService navigationService, IMetricsHelper metricsHelper)
        {
            _navigationService = navigationService;
            _metricsHelper = metricsHelper;
            SetTabsTitles();
        }


        private void SetTabsTitles()
        {
            var spaceWidth = _metricsHelper.GetWidthOfString("i i") - _metricsHelper.GetWidthOfString("ii");
            var mineTitleWidth = _metricsHelper.GetWidthOfString("MINE") + 2 * spaceWidth;
            var starredTitleWidth = _metricsHelper.GetWidthOfString("STARRED");
            var allTitleWidth = _metricsHelper.GetWidthOfString("ALL") + 2 * spaceWidth;
            var restOfSpace = App.ScreenWidth - ( mineTitleWidth + starredTitleWidth + allTitleWidth ) - 7;
            var minePageTabWidth = restOfSpace / 2;
            var allPageTabWidth = restOfSpace - minePageTabWidth;
            var amountOfMineTabSpaces = minePageTabWidth / spaceWidth;
            amountOfMineTabSpaces = amountOfMineTabSpaces - amountOfMineTabSpaces / 3;
            var amountOfAllTabSpaces = allPageTabWidth / spaceWidth;
            amountOfAllTabSpaces = amountOfAllTabSpaces - amountOfAllTabSpaces / 3;
            MinePageTitle = MinePageTitle.PadLeft(amountOfMineTabSpaces - 2, ' ') + "MINE" + "  ";
            StarredPageTitle = "STARRED";
            AllPageTitle = "  " + "ALL" + AllPageTitle.PadRight(amountOfAllTabSpaces - 2, ' ');
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if ( !parameters.ContainsKey("Session") ) return;

            _session = parameters["Session"] as Session;
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }
    }
}
