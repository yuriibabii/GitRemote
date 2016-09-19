using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(NativeCell), typeof(NativeCellRendererDroid))]
namespace GitRemote.Droid.Renderers
{
    public class NativeCellRendererDroid : ViewCellRenderer
    {
        private NativeCell _formsView;

        protected override View GetCellCore(Cell item, View convertView,
            ViewGroup parent, Context context)
        {
            var formsView = ( NativeCell )item;
            var view = convertView ??
                       ( context as Activity )?.LayoutInflater.Inflate(Resource.Layout.NativeCellAndroid, null);

            if ( view == null ) return null;

            view.FindViewById<TextView>(Resource.Id.TextLabel).Text = formsView.UserName;
            if ( formsView.ChangeBackgroundWhenSelected )
                view.FocusChange += OnFocusChanged;

            _formsView = formsView;
            return view;
        }

        private void OnFocusChanged(object obj, View.FocusChangeEventArgs eventArgs)
        {
            Drawable drawable = eventArgs.HasFocus
                ? new ColorDrawable(_formsView.BackgroundColorWhenSelected.ToAndroid())
                : new ColorDrawable(Color.White.ToAndroid());

            ( ( View )obj ).Background = drawable;
        }
    }
}