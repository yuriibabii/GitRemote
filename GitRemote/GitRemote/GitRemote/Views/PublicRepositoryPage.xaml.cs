using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Services.MessageService;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Views
{
    public partial class PublicRepositoryPage
    {
        private readonly IEventAggregator _eventAggregator;

        public PublicRepositoryPage(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            InitializeComponent();

            _eventAggregator
                .GetEvent<SetCurrentTabWithTitle>()
                .Subscribe(OnSetCurrentTabWithTitle);
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

            _eventAggregator
                .GetEvent<PublicReposCurrentTabChanged>()
                .Publish(CurrentPage.Title);
        }

        private void OnSetCurrentTabWithTitle(string title)
        {
            foreach ( var child in Children )
            {
                if ( child.Title == title )
                {
                    CurrentPage = child;
                    return;
                }
            }
        }

        protected override void OnDisappearing()
        {
            _eventAggregator
                .GetEvent<SetCurrentTabWithTitle>()
                .Unsubscribe(OnSetCurrentTabWithTitle);

            base.OnDisappearing();
        }
    }
}
