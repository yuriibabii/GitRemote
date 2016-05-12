using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitRemote.Helpers;
using GitRemote.Views;
using Xamarin.Forms;

namespace GitRemote
{
    public class App : Application
    {
        public static NavigationPage Navigation;

        public App()
        {
            Navigation = new NavigationPage(new ProfilePage());
            MainPage = Navigation;
            
        }
       
    }
}
