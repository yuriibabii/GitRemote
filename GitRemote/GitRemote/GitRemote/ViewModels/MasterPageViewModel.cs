using GitRemote.GitHub;
using GitRemote.Models;
using GitRemote.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;

namespace GitRemote.ViewModels
{
    public class MasterPageViewModel : BindableBase
    {

        #region Constants
        private const string GistsPageImagePath = "ic_code_black_24dp.png";
        private const string IssueDashboardPageImagePath = "ic_slow_motion_video_black_24dp.png";
        private const string BookmarksPageImagePath = "ic_bookmark_black_24dp.png";
        private const string ReportAnIssuePageImagePath = "ic_error_outline_black_24dp.png";
        #endregion

        private MasterPageMenuItemModel _menuItemSelectedProperty;

        public MasterPageMenuItemModel MenuItemSelectedProperty
        {
            get { return _menuItemSelectedProperty; }
            set { SetProperty(ref _menuItemSelectedProperty, value); }
        }
        ////#endregion

        private readonly INavigationService _navigationService;

        public ObservableCollection<MasterPageMenuItemModel> MenuItems;

        public DelegateCommand MenuItemSelected { get; }
        public DelegateCommand ExitCommand { get; }

        public MasterPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            MenuItemSelected = new DelegateCommand(OnMenuItemSelected);
            ExitCommand = new DelegateCommand(OnExit);

            //MasterProfileGrayHeaderBounds = new Rectangle(0, 0, 1, 0.175);
            //MasterProfileImageBounds = new Rectangle(16, 16, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
            //MasterProfileNameBounds = new Rectangle(16, 70, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);
            //MasterMenuBounds = new Rectangle(16, 100 + 10, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);

            MenuItems = new ObservableCollection<MasterPageMenuItemModel>
            {
                new MasterPageMenuItemModel {Name = "GistsPage", ImageSource = GistsPageImagePath},
                new MasterPageMenuItemModel {Name = "IssueDashboardPage", ImageSource = IssueDashboardPageImagePath},
                new MasterPageMenuItemModel {Name = "BookmarksPage", ImageSource = BookmarksPageImagePath},
                new MasterPageMenuItemModel {Name = "ReportAnIssuePage", ImageSource = ReportAnIssuePageImagePath}
            };
        }

        private void OnMenuItemSelected()
        {
            if ( MenuItemSelectedProperty == null ) return;
            _navigationService.NavigateAsync(MenuItemSelectedProperty.Name, animated: false);
            MenuItemSelectedProperty = null;
        }

        private void OnExit()
        {
            UserManager.SetLastUser(string.Empty);
            var navigationStack = new Uri("https://Necessary/" + $"{nameof(StartPage)}", UriKind.Absolute);
            _navigationService.NavigateAsync(navigationStack, animated: false);
        }
    }
}
