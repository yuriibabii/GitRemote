using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Views
{
    public partial class PublicRepositoryPage
    {
        public PublicRepositoryPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, SetCurrentTabWithTitle, OnSetCurrentTabWithTitle);
        }

        protected override void OnCurrentPageChanged()
        {
            MessagingCenter.Send(CurrentPage.Title, PublicReposCurrentTabChanged);
            base.OnCurrentPageChanged();
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
            MessagingCenter.Unsubscribe<string>(this, SetCurrentTabWithTitle);
            base.OnDisappearing();
        }
    }
}
