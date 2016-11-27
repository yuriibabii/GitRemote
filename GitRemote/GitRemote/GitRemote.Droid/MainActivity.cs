using System.Threading.Tasks;
using Android.App;
using Android.Content.PM;
using Android.OS;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Droid;
using GitRemote.Services;
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
            App.ScreenWidth = ( int )( Resources.DisplayMetrics.WidthPixels / Resources.DisplayMetrics.Density );
            FlowListView.Init();
            CachedImageRenderer.Init();
            ToolbarResource = Resource.Layout.toolbar;
            TabLayoutResource = Resource.Layout.tabs;

            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            LoadApplication(new App());

            MessagingCenter.Subscribe<string>(this, ConstantsService.Messages.PressHardwareBack, PressHardwareBack);

            //var tv = FindViewById(Resource.Repository.mySearchView) as SearchView;
            //var tb = ( Toolbar )tv?.Parent;
            //if ( tb != null ) tb.Elevation = 20f;
        }

        public override void OnBackPressed()
        {
            Device.BeginInvokeOnMainThread(() => MessagingCenter.Send("JustIgnore", ConstantsService.Messages.HardwareBackPressed));
        }

        private void PressHardwareBack(string sender)
        {
            base.OnBackPressed();
        }

        
    }
}

