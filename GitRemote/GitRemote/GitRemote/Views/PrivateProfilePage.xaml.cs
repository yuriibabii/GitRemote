using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Views
{
    public partial class PrivateProfilePage
    {
        public PrivateProfilePage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, HideMasterPage, OnHideMasterPage);
        }

        private void OnHideMasterPage(string ignore)
        {
            IsPresented = false;
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<string>(this, HideMasterPage);
            base.OnDisappearing();
        }
    }
}
