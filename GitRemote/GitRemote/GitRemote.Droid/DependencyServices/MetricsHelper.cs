using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(MetricsHelper))]
namespace GitRemote.Droid.DependencyServices
{
    public class MetricsHelper : IMetricsHelper
    {
        public int GetWidthOfString(string text, float textSize = 14)
        {
            var bounds = new Rect();
            var textView = new TextView(Forms.Context) { TextSize = textSize };
            textView.Paint.GetTextBounds(text, 0, text.Length, bounds);
            var length = bounds.Width();
            var lenghtInDensity = Convert.ToInt32(length / Resources.System.DisplayMetrics.ScaledDensity);
            return lenghtInDensity;
        }


    }

}