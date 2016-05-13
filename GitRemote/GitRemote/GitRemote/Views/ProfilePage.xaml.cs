using Xamarin.Forms;

namespace GitRemote.Views
{
    public partial class ProfilePage : MasterDetailPage
    {
        public static NavigationPage NavigationPage;
        public static NavigationPage nav;
        public ProfilePage()
        {
            InitializeComponent();  
            nav = new NavigationPage(new DetailPage());
            NavigationPage.SetHasNavigationBar(nav, false);
            Detail = nav;
            
            Master = new MasterPage();
            //NavigationPage = new NavigationPage(new MasterPage());
            //NavigationPage.Title = "Nessesacy";
            //Master = NavigationPage;
            // Master = new NavigationPage(new MasterPage()) {Title = "Nessesary"};          
        }
    }
}
