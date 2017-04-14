using Android.Content.Res;
using Android.Graphics;
using Android.Widget;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using System;
using Android.Util;
using Android.Views;
using Xamarin.Forms;
using Application = Android.App.Application;
using Font = Android.Renderscripts.Font;

[assembly: Dependency(typeof(MetricsHelper))]
namespace GitRemote.Droid.DependencyServices
{
    public class MetricsHelper : IMetricsHelper
    {
        private static Typeface _textTypeface;

        public double GetLabelWidth(string text, double maxWidth, double fontSize = 14)
        {
            return GetRectangle(text, maxWidth, fontSize).Width;
        }

        public double GetLabelHeight(string text, double maxWidth, double fontSize = 14)
        {
            return GetRectangle(text, maxWidth, fontSize).Height;
        }

        private Rectangle GetRectangle(string text, double width, double fontSize)
        {
            var textView = new TextView(Application.Context) { Typeface = GetTypeface(null) };
            textView.SetText(text, TextView.BufferType.Normal);
            textView.SetTextSize(ComplexUnitType.Px, (float)fontSize);

            var widthMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec((int)width, MeasureSpecMode.AtMost);
            var heightMeasureSpec = Android.Views.View.MeasureSpec.MakeMeasureSpec(0, MeasureSpecMode.Unspecified);
            textView.Measure(widthMeasureSpec, heightMeasureSpec);
            return new Rectangle { Width = textView.MeasuredWidth, Height = textView.MeasuredHeight };
        }

        private static Typeface GetTypeface(string fontName)
        {
            if (fontName == null)
                return Typeface.Default;

            return _textTypeface ?? (_textTypeface = Typeface.Create(fontName, TypefaceStyle.Normal));
        }


    }



}