using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class ProfilePage
    {
        public ProfilePage()
        {
            InitializeComponent();
            App.Navigator = new NavigationPage(new DetailPage());
            NavigationPage.SetHasNavigationBar(App.Navigator, false);
            Detail = App.Navigator;
            Master = new MasterPage();
        }
    }
}
