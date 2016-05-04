using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Util;
using Android.Support.V4.Content;


namespace GitRemote.Droid
{
    [Activity(Label = "GitRemote", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        Android.Support.V7.Widget.SearchView searchView;
        protected override void OnCreate(Bundle bundle)
        {
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            LoadApplication(new App());
            
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //change menu_search to your name
           
                this.MenuInflater.Inflate(Resource.Menu.menu_home, menu);
                
                var searchItem = menu.FindItem(Resource.Id.action_search);
                var provider = MenuItemCompat.GetActionView(searchItem);
                searchView = provider.JavaCast<Android.Support.V7.Widget.SearchView>();

                searchView.QueryTextSubmit += (sender, args) => {

                    var view = sender as Android.Support.V7.Widget.SearchView;
                    if ( view != null )
                        view.ClearFocus();
                };
            return base.OnCreateOptionsMenu(menu);
        }
    }
}

