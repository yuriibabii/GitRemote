using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitRemote.Models;
using GitRemote.Views;
using GitRemote.Views.MasterPageViews;
using Xamarin.Forms;

namespace GitRemote.Helpers
{
    public static class MasterNavigation
    {
        
        private static string CurrentPage { get; set; }

        private static bool _exist;

        public static bool Exist
        {
            get { return _exist; }
            set { if ( ( _exist == false ) && value ) _exist = true; }
        }

        public static async void NavigateAsync(MenuType id)
        {
            switch ( id )
            {
                case MenuType.Gists:
                    await App.Navigator.PushAsync(new GistsPage());                    
                    break;
                case MenuType.IssueDashboard:
                    await App.Navigator.PushAsync(new IssueDashboardPage());
                    break;
                case MenuType.Bookmarks:
                    await App.Navigator.PushAsync(new BookmarksPage());
                    break;
                case MenuType.ReportAnIssue:
                    await App.Navigator.PushAsync(new ReportAnIssuePage());
                    break;
            }
            CurrentPage = id.ToString();
        }
    }
}


