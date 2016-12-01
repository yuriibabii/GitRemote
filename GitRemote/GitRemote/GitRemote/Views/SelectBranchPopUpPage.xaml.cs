using GitRemote.Services;
using System;
using System.Linq;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class SelectBranchPopUpPage
    {
        private int _index;
        public SelectBranchPopUpPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.ScrollToActivatedBranchItem, ScrollToElement);
        }

        private void ScrollToElement(string index)
        {
            _index = Convert.ToInt32(index);
            BranchesAndTagsList.FlowItemAppearing += Scroll;
            MessagingCenter.Unsubscribe<string>(this, ConstantsService.Messages.ScrollToActivatedBranchItem);
        }

        private void Scroll(object sender, ItemVisibilityEventArgs args)
        {
            BranchesAndTagsList.FlowItemAppearing -= Scroll;
            BranchesAndTagsList.ScrollTo(BranchesAndTagsList.ItemsSource.Cast<object>()
                   .ToList()[_index], ScrollToPosition.Center, true);
        }
    }
}
