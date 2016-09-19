using Android.Graphics.Drawables;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: Dependency(typeof(BackgroundHelper))]
namespace GitRemote.Droid.DependencyServices
{
    public class BackgroundHelper : IBackgroundHelper
    {
        private static Android.Views.View _view;
        public void ChangeBackgroundColor(Color color)
        {
            if ( _view == null ) return;
            Drawable drawable = new ColorDrawable(color.ToAndroid());
            _view.Background = drawable;
        }

        public static void SaveView(Android.Views.View view)
        {
            _view = view;
        }
    }
}