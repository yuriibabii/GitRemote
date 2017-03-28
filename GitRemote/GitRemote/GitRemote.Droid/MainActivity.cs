using Android.App;
using Android.Content.PM;
using Android.OS;
using DLToolkit.Forms.Controls;
using FFImageLoading.Forms.Droid;
using GitRemote.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using static GitRemote.Services.MessageService.Messages;

namespace GitRemote.Droid
{
    [Activity(Label = "GitRemote", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize |
        ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : FormsAppCompatActivity
    {
        private bool _isExecuteHardwareBack = true;

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

#if DEBUG
            UISleuth.Inspector.Init();
            // optional
            //UISleuth.Inspector.ShowAcceptingConnections();
#endif
            MessagingCenter.Subscribe<string>(this, SetIsExecuteHardwareBack, OnSetExecuteHardwareBack);
        }

        private void OnSetExecuteHardwareBack(string s)
        {
            if ( StringService.CheckForNullOrEmpty(s) )
                _isExecuteHardwareBack = Convert.ToBoolean(s);
        }

        public override void OnBackPressed()
        {
            MessagingCenter.Send("JustIgnore", HardwareBackPressed);

            if ( !_isExecuteHardwareBack )
            {
                _isExecuteHardwareBack = true;
                return;
            }

            base.OnBackPressed();
        }

        protected override void OnDestroy()
        {
            MessagingCenter.Unsubscribe<string>(this, SetIsExecuteHardwareBack);
            base.OnDestroy();
        }
    }
}

