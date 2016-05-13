using GitRemote.Views;
using Xamarin.Forms;

namespace GitRemote
{
    public class App : Application
    {
        public static NavigationPage Navigator;
        public static ProfilePage MDP;

        public App()
        {
            MDP = new ProfilePage();
            //MainPage = new ProfilePage();
            MainPage = MDP;
        }
    }
}
