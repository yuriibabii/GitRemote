using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GitRemote
{
    public partial class GeneralPage : MasterDetailPage
    {
        public GeneralPage()
        {
            InitializeComponent();
            this.Detail.Navigation.PushAsync(new UpperTabs());

        }
    }
}
