using GitRemote.Services;
using Prism.Events;
using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Views
{
    public partial class PrivateProfilePage
    {
        private readonly IEventAggregator _eventAggregator;

        public PrivateProfilePage(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<MessageService.HideMasterPage>()
                .Subscribe(OnHideMasterPage);
        }

        private void OnHideMasterPage(string ignore)
        {
            IsPresented = false;
        }

        protected override void OnDisappearing()
        {
            _eventAggregator
                .GetEvent<MessageService.HideMasterPage>()
                .Unsubscribe(OnHideMasterPage);

            base.OnDisappearing();
        }
    }
}
