using System;
using System.Collections.Generic;
using GitRemote.Helpers;
using GitRemote.Models;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class MasterPage : ContentPage
    {
        private List <MasterPageItem> _menuItems;


        public MasterPage()
        {

            InitializeComponent();

            AbsoluteLayout.SetLayoutBounds(MasterProfileImage, new Rectangle(16, 16, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(MasterProfileImage, AbsoluteLayoutFlags.None);
            
            AbsoluteLayout.SetLayoutBounds(MasterProfileName, new Rectangle(16, 16 + 50 + 5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(MasterProfileImage, AbsoluteLayoutFlags.None);

            AbsoluteLayout.SetLayoutBounds(ListViewMenu, new Rectangle(0, 16 + 50 + 5 + 16 + 16 + 30, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(MasterProfileImage, AbsoluteLayoutFlags.None);

            BindingContext = new MasterPageItem();

            _menuItems = new List<MasterPageItem>
            {
                new MasterPageItem {MasterItemName = "Gists", MenuType = MenuType.Gists, MasterItemImage = "ic_code_black_24dp.png"},
                new MasterPageItem {MasterItemName = "Issue Dashboard", MenuType = MenuType.IssueDashboard, MasterItemImage = "ic_slow_motion_video_black_24dp.png"},
                new MasterPageItem {MasterItemName = "Bookmarks", MenuType = MenuType.Bookmarks, MasterItemImage = "ic_bookmark_black_24dp.png"},
                new MasterPageItem {MasterItemName = "Report an issue", MenuType = MenuType.ReportAnIssue, MasterItemImage = "ic_error_outline_black_24dp.png"}
            };

            ListViewMenu.ItemsSource = _menuItems;
            ListViewMenu.SelectedItem = null;

            ListViewMenu.ItemSelected += (sender, e) =>
            {
                if ( ListViewMenu.SelectedItem == null )
                    return;

                MasterNavigation.Exist = true;
                
                MasterNavigation.NavigateAsync(( ( MasterPageItem )e.SelectedItem ).MenuType);
            };
        }
    }
}
