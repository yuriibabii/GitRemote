using Android.Graphics;
using Android.Text.Util;
using Android.Widget;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HyperLinkLabel), typeof(HyperLinkLabelRenderer))]
namespace GitRemote.Droid.Renderers
{
    public class HyperLinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            if ( e.OldElement != null ) return;

            var nativeEditText = Control;

            var view = ( HyperLinkLabel )Element;

            SetUserInterfaceOptions(view, Control);

            Linkify.AddLinks(nativeEditText, MatchOptions.All);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
        {
            if ( eventArgs.PropertyName == "IsUnderline" )
                Control.PaintFlags = ( ( HyperLinkLabel )sender ).IsUnderline
                    ? Control.PaintFlags | PaintFlags.UnderlineText
                    : Control.PaintFlags &= ~PaintFlags.UnderlineText;
            else
            if (eventArgs.PropertyName == "TextColor")
            {
                var label = (HyperLinkLabel) sender;
                Control.SetTextColor(new Android.Graphics.Color(label.TextColor.ToAndroid()));
            }
               
        }

        /// <summary>
        /// Sets options that are described for this control in Forms project 
        /// </summary>
        /// <param name="view">View from Forms project</param>
        /// <param name="control">Control that is a view in Android project</param>
        private static void SetUserInterfaceOptions(HyperLinkLabel view, TextView control)
        {
            control.PaintFlags = view.IsUnderline
                    ? control.PaintFlags | PaintFlags.UnderlineText
                    : control.PaintFlags &= ~PaintFlags.UnderlineText;
        }
    }
}