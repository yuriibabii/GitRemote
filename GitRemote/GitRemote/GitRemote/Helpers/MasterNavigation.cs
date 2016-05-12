using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GitRemote.Models;
using GitRemote.Views;
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
            set { if ((_exist == false) && value) _exist = true; }
        }

        public static async void NavigateAsync(MenuType id)
        {
            switch (id)
            {
                case MenuType.Gists:
                    await App.Navigation.PushAsync(new Views.MasterPageViews.GistsPage());
                    break;
                case MenuType.IssueDashboard:
                    await App.Navigation.PushAsync(new Views.MasterPageViews.IssueDashboardPage());
                    break;
                case MenuType.Bookmarks:
                    await App.Navigation.PushAsync(new Views.MasterPageViews.BookmarksPage());
                    break;
                case MenuType.ReportAnIssue:
                    await App.Navigation.PushAsync(new Views.MasterPageViews.ReportAnIssuePage());
                    break;
            }
            CurrentPage = id.ToString();
        }        
    }
}


