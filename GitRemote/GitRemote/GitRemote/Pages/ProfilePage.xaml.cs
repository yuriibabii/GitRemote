using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GitRemote
{
    public partial class ProfilePage : MasterDetailPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            Detail = new NavigationPage(new Pages.DetailPage());
        }
    }
}
