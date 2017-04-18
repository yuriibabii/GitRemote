using System;
using GitRemote.DI;
using GitRemote.GitHub;
using GitRemote.GitHub.Managers;
using GitRemote.Models;
using GitRemote.Services;
using GitRemote.Views;
using Nito.Mvvm;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GitRemote.Common;
using MvvmHelpers;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Models.PrivateNewsModel;
using static GitRemote.Services.MessageService.MessageModels;
using static GitRemote.Services.MessageService.Messages;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.ViewModels
{
    public class PrivateNewsPageViewModel : BindableBase
    {
        #region Commands

        public DelegateCommand ItemTappedCommand => new DelegateCommand(OnItemTapped);
        public DelegateCommand<object> LoadMoreCommand => new DelegateCommand<object>(OnLoadMore, CanLoadMore);
        public DelegateCommand RefreshCommand => new DelegateCommand(OnRefresh, CanRefresh);

        #endregion

        #region Properties

        public PrivateNewsModel TappedItem { get; set; }
        public GridLength ColumnWidth => new GridLength(App.ScreenWidth < ConstantsService.MaxNormalWidthForTitle
              ? App.ScreenWidth - ConstantsService.OtherWidth
              : ConstantsService.MaxNormalWidthForTitle);

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                var changed = SetProperty(ref _isBusy, value);
                if (changed) RefreshCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        private int _pageNumber = 1;
        private readonly Session _session;
        public NotifyTask<ObservableRangeCollection<PrivateNewsModel>> PrivateNews { get; set; }
        private readonly IEventAggregator _eventAggregator;
        private readonly PrivateNewsManager _manager;

        public PrivateNewsPageViewModel(ISecuredDataProvider securedDataProvider, IEventAggregator eventAggregator)
        {
            var store = securedDataProvider.Retreive(ConstantsService.ProviderName, UserManager.GetLastUser());
            var lastUser = UserManager.GetLastUser();
            var token = store.Properties.First().Value;
            var privateFeedUrl = store.Properties[PrivateFeedUrl];
            _session = new Session(lastUser, token, privateFeedUrl);
            _manager = new PrivateNewsManager(_session);
            PrivateNews = NotifyTask.Create(_manager.GetPrivateNews());
            _eventAggregator = eventAggregator;
        }

        private void OnLoadMore(object model)
        {
            _pageNumber++;
            Device.BeginInvokeOnMainThread(async () =>
            {
                IsBusy = true;
                var newItems = await _manager.GetPrivateNews(_pageNumber);
                PrivateNews.Result.AddRange(newItems);
                IsBusy = false;
            });
        }

        private void OnRefresh()
        {
            _pageNumber = 1;
            Device.BeginInvokeOnMainThread(() =>
            {
                IsBusy = true;
                PrivateNews = NotifyTask.Create(_manager.GetPrivateNews());
                var loadTask = PrivateNews.TaskCompleted;
                loadTask.ContinueWith(task =>
                {
                    RaisePropertyChanged(nameof(PrivateNews));
                    IsBusy = false;
                });
            });
        }

        private bool CanLoadMore(object model)
        {
            if (IsBusy) return false;

            if (PrivateNews.Result.Count < 1) return false;

            var news = (PrivateNewsModel)model;
            return PrivateNews.Result[PrivateNews.Result.Count - 1].Title == news.Title;
        }

        private bool CanRefresh()
        {
            return !IsBusy;
        }

        private void OnItemTapped()
        {
            var splited = TappedItem.Target.Split('/');
            var ownerName = TappedItem.ActionType == Enums.ActionTypes.Forked
                ? TappedItem.Perfomer
                : splited[0];

            var reposName = splited[1];

            //TODO
            //Not implemented navigation to IssuePage when content of news is Issue.
            //Because of lack of this page.

            var parameters = new NavigationParameters
            {
                { OwnerName, ownerName},
                { ReposName, reposName},
                { nameof(Session), _session},
            };

            var path = $"{nameof(PublicRepositoryPage)}";

            _eventAggregator
                .GetEvent<MessageService.DoNavigation>()
                .Publish(new DoNavigationModel(path, parameters));
        }


    }
}
