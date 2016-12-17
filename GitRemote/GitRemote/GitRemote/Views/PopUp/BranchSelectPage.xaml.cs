using GitRemote.Services;
using System;
using System.Linq;
using Xamarin.Forms;

namespace GitRemote.Views.PopUp
{
    public partial class BranchSelectPage
    {
        private int _index;
        public BranchSelectPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, MessageService.Messages.ScrollToActivatedBranchItem, ScrollToElement);
        }

        private void ScrollToElement(string index)
        {
            _index = Convert.ToInt32(index);
            BranchesAndTagsList.FlowItemAppearing += Scroll;
            MessagingCenter.Unsubscribe<string>(this, MessageService.Messages.ScrollToActivatedBranchItem);
        }

        private void Scroll(object sender, ItemVisibilityEventArgs args)
        {
            BranchesAndTagsList.FlowItemAppearing -= Scroll;
            BranchesAndTagsList.ScrollTo(BranchesAndTagsList.ItemsSource.Cast<object>()
                   .ToList()[_index], ScrollToPosition.Center, true);
        }
    }
}
