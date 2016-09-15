
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(NativeCell), typeof(NativeCellRendererDroid))]
namespace GitRemote.Droid.Renderers
{
    public class NativeCellRendererDroid : ViewCellRenderer
    {
        protected override Android.Views.View GetCellCore(Xamarin.Forms.Cell item, Android.Views.View convertView,
            ViewGroup parent, Context context)
        {
            var formsView = ( NativeCell )item;
            var view = convertView ??
                       ( context as Activity )?.LayoutInflater.Inflate(Resource.Layout.NativeCellAndroid, null);
            if ( view != null )
                view.FindViewById<TextView>(Resource.Id.TextLabel).Text = formsView.UserName;

            return view;
        }
    }
}