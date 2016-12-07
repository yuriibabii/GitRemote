using Xamarin.Forms;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Views
{
    public partial class PublicRepositoryPage
    {
        public PublicRepositoryPage()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            MessagingCenter.Send(CurrentPage.Title, PublicReposCurrentTabChanged);
            base.OnCurrentPageChanged();
        }
    }
}
