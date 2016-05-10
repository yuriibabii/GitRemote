using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitRemote.Models;
using Xamarin.Forms;

namespace GitRemote.Views
{
    public class RootPage:MasterDetailPage 
    {
        private Dictionary<MenuType, NavigationPage> Pages { get; set; }

        public RootPage()
        {
            Pages = new Dictionary<MenuType, NavigationPage>();
            Master = new MasterPage(this);

            NavigateAsync(MenuType.Detail);

            //InvalidateMeasure();
        }

        public async Task NavigateAsync(MenuType id)
        {
            Page newPage;

            if ( !Pages.ContainsKey(id) )
            {

                switch ( id )
                {
                    case MenuType.Gists:
                        Pages.Add(id, new NavigationPage(new ContentPage()));
                        break;
                    case MenuType.IssueDashboard:
                        Pages.Add(id, new NavigationPage(new ContentPage()));
                        break;
                    case MenuType.Bookmarks:
                        Pages.Add(id, new NavigationPage(new ContentPage()));
                        break;
                    case MenuType.ReportAnIssue:
                        Pages.Add(id, new NavigationPage(new ContentPage()));
                        break;
                    case MenuType.Detail:
                        Pages.Add(id, new NavigationPage(new DetailPage()));
                        break;
                }
            }

            newPage = Pages[id];
            if (newPage == null)
                return;

            Detail = newPage;
        }
    }
}
