using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class FileExplorerPage : ContentPage
    {
        public FileExplorerPage()
        {
            InitializeComponent();
            MessagingCenter.Send(PathParts, "TakePathPartsGrid");
        }
    }
}
