using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
namespace GitRemote.Droid
{
    [Activity(Label = "GitRemote", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize |
        ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }
}

