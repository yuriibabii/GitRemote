using Android.Graphics;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FontIcon), typeof(FontIconRenderer))]
namespace GitRemote.Droid.Renderers
{
    public class FontIconRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                try
                {
                    Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, Element.FontFamily + ".ttf");
                }
                catch (Java.Lang.RuntimeException ex)
                {
                    if (ex.Message == "native typeface cannot be made" || ex.Message == "Font asset not found FontAwesome.ttf")
                        Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, Element.FontFamily + ".otf");
                    else
                        throw;
                }
            }
        }

    }
}