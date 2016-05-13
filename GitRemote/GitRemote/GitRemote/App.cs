using GitRemote.Views;
using Xamarin.Forms;

namespace GitRemote
{
    public class App : Application
    {
        
        public App()
        {
            //Navigation = new NavigationPage(new ProfilePage());
            //MainPage = Navigation;
            //MainPage = new Views.ProfilePage();
            MainPage = new ProfilePage();
        }
    }
}
