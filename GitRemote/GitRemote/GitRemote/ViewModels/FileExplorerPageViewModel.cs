using GitRemote.CustomClasses;
using GitRemote.DI;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GitRemote.ViewModels
{
    public class FileExplorerPageViewModel : BindableBase, INavigationAware
    {
        #region Props
        public ObservableCollection<FileExplorerModel> FileTree { get; set; }
        public DelegateCommand ListItemTappedCommand { get; }
        public FileExplorerModel LastTappedItem { get; set; }
        public string PathPartsIcon => FontIconsService.Octicons.SubModule;
        public GridLength PathPartsRowWidth => new GridLength(App.ScreenWidth - 40 - 10 - 10); // fontIcon, spaceBefore, spaceAfter
        public bool PathPartsGridIsVisible
        {
            get { return _pathPartsGridIsVisible; }
            set { SetProperty(ref _pathPartsGridIsVisible, value); }
        }
        #endregion

        #region Fields
        private readonly INavigationService _navigationService;
        public NotifyTask SetCurrentBranchTask;
        public NotifyTask SetTreeTask;
        private readonly FileTreeManager _manager;
        private Grid _pathPartsGrid;
        private bool _pathPartsGridIsVisible;
        private readonly List<bool> _layoutIsFull = new List<bool>();
        private int _currentPathPartIndex;
        #endregion

        public FileExplorerPageViewModel(INavigationService navigationService)
        {
            _manager = new FileTreeManager("UniorDev", "GitRemote");
            SetCurrentBranchTask = NotifyTask.Create(_manager.SetCurrentBranchAsync());
            SetCurrentBranchTask.TaskCompleted.ContinueWith(branchTask =>
            {
                SetTreeTask = NotifyTask.Create(_manager.SetTreeAsync());
                SetTreeTask.TaskCompleted.ContinueWith(treeTask =>
                {
                    FileTree = _manager.GetFiles("GitRemote/");
                    OnPropertyChanged(nameof(FileTree));
                });
            });

            _navigationService = navigationService;
            ListItemTappedCommand = new DelegateCommand(OnListItemTapped);

            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.HardwareBackPressed, OnHardwareBackPressed);
            MessagingCenter.Subscribe<Grid>(this, "TakePathPartsGrid", SetPathPartsGrid);

        }

        private void SetPathPartsGrid(Grid grid)
        {
            _pathPartsGrid = grid;

            var layout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Horizontal,
                Spacing = 3
            };

            #region LinkLabel

            var pathPart = new HyperLinkLabel
            {
                Text = "GitRemote",
                FontSize = 16,
                TextColor = Color.FromHex("3366BB"),
                IsUnderline = true,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation,
                GestureRecognizers =
                {
                    new TapGestureRecognizer
                    {
                        NumberOfTapsRequired = 1,
                        Command = new DelegateCommand(OnPathPartTapped),
                        CommandParameter = _currentPathPartIndex
                    }
                }
            };

            #endregion

            #region Slash

            var slash = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "/",
                FontSize = 16,
                TextColor = Color.Black,
            };

            #endregion

            layout.Children.Add(pathPart);
            layout.Children.Add(slash);
            _pathPartsGrid.RowDefinitions = new RowDefinitionCollection { new RowDefinition { Height = 20 } };
            _pathPartsGrid.Children.Add(layout, 1, 0);
            _layoutIsFull.Add(false);
        }

        private void OnPathPartTapped()
        {

        }

        private void OnListItemTapped()
        {
            if ( LastTappedItem.IsFolder )
            {
                FileTree = _manager.GetFiles(LastTappedItem.Name + "/");
                OnPropertyChanged(nameof(FileTree));
                PathPartsGridIsVisible = true;
                _currentPathPartIndex++;
                AddPathPart(LastTappedItem.Name);
            }
        }

        private void AddPathPart(string pathPartName)
        {
            var layout = ( StackLayout )_pathPartsGrid.Children[_layoutIsFull.Count];
            var layoutText = string.Empty;

            foreach ( var child in layout.Children )
            {
                layoutText = string.Concat(layoutText, ( ( Label )child ).Text);
            }

            var size = DependencyService.Get<IMetricsHelper>()
                .GetWidthOfString(layoutText + pathPartName + "/", ( float )( ( Label )layout.Children[0] ).FontSize);

            #region LinkLabel
            var pathPart = new HyperLinkLabel
            {
                Text = pathPartName,
                FontSize = 16,
                TextColor = Color.FromHex("3366BB"),
                IsUnderline = true,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                LineBreakMode = LineBreakMode.TailTruncation,
                GestureRecognizers =
                            {
                                new TapGestureRecognizer
                                {
                                    NumberOfTapsRequired = 1,
                                    Command = new DelegateCommand(OnPathPartTapped),
                                    CommandParameter = _currentPathPartIndex
                                }
                            }
            };


            #endregion

            #region Slash

            var slash = new Label
            {
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "/",
                FontSize = 16,
                TextColor = Color.Black,
            };

            #endregion

            if ( layout.Spacing * ( layout.Children.Count + 1 ) + size < PathPartsRowWidth.Value )
            {
                layout.Children.Add(pathPart);
                layout.Children.Add(slash);
            }
            else
            {
                _layoutIsFull[_layoutIsFull.Count - 1] = true;

                var newLayout = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.Center,
                    Orientation = StackOrientation.Horizontal,
                    Spacing = 3
                };

                newLayout.Children.Add(pathPart);
                newLayout.Children.Add(slash);
                _pathPartsGrid.RowDefinitions = new RowDefinitionCollection { new RowDefinition { Height = 20 } };
                _pathPartsGrid.Children.Add(newLayout, 1, _layoutIsFull.Count);
                _layoutIsFull.Add(false);
            }
        }

        private void OnHardwareBackPressed(string sender)
        {
            if ( _manager.PopUpExplorer() )
            {
                FileTree = _manager.GetFiles(string.Empty);
                OnPropertyChanged(nameof(FileTree));
                _currentPathPartIndex--;
                if ( _currentPathPartIndex == 0 )
                    PathPartsGridIsVisible = false;

            }
            else
                MessagingCenter.Send("JustIgnore", ConstantsService.Messages.PressHardwareBack);
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
            MessagingCenter.Unsubscribe<string>(this, ConstantsService.Messages.HardwareBackPressed);
        }
    }
}
