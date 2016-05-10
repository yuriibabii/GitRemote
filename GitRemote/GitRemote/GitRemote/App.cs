using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GitRemote.Views;
using Xamarin.Forms;

namespace GitRemote
{
    public class App : Application
    {
        public App()
        {
            MainPage = new RootPage();
            //MainPage = new ProfilePage();
            //MainPage = new ContentPage();
        }
       
    }
}
