using Android.Views;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CodeEntry), typeof(CodeEntryRenderer))]

namespace GitRemote.Droid.Renderers
{
    public class CodeEntryRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if ( e.OldElement != null ) return;

            var ctrl = CreateNativeControl();
            ctrl.RequestFocus();
            SetNativeControl(ctrl);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);


        }
        protected override View CreateNativeControl()
        {
            return LayoutInflater.From(Context).Inflate(Resource.Layout.CodeInputLayout, null);
        }
    }
}