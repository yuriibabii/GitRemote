using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GitRemote
{
    public class App : Application
    {
        public App()
        {
            MainPage = new GeneralPage();
        }
    }
}
