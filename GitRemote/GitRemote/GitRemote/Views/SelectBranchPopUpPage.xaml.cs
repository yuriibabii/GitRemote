using DLToolkit.Forms.Controls;
using GitRemote.Models;
using GitRemote.Services;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class SelectBranchPopUpPage
    {
        public SelectBranchPopUpPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.ScrollToActivatedBranchItem, ScrollToElement);
        }

        private void ScrollToElement(string sender)
        {
            foreach (var o in BranchesAndTagsList.FlowItemsSource)
            {
                if (((SelectBranchPopUpModel)o).Name == sender)
                {
                    BranchesAndTagsList.ScrollTo(o, ScrollToPosition.Center, true);
                    break;
                }
            }

            MessagingCenter.Unsubscribe<string>(this, ConstantsService.Messages.ScrollToActivatedBranchItem);
        }
    }
}
