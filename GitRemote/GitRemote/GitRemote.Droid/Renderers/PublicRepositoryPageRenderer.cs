using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Views;
using GitRemote.Droid.Renderers;
using GitRemote.ViewModels;
using GitRemote.Views;
using Java.Net;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using static Android.Graphics.Bitmap.Config;
using Color = Android.Graphics.Color;
using Toolbar = Android.Support.V7.Widget.Toolbar;

[assembly: ExportRenderer(typeof(PublicRepositoryPage), typeof(PublicRepositoryPageRenderer))]

namespace GitRemote.Droid.Renderers
{
    public class PublicRepositoryPageRenderer : TabbedPageRenderer
    {
        private Activity GetActivity
        {
            get
            {
                try
                {
                    return ( Activity )Context;
                }
                catch
                {
                    return null;
                }
            }
        }

        private Toolbar GetToolbar
        {
            get
            {
                var toolbar = GetActivity?.FindViewById<Toolbar>(Resource.Id.toolbar);

                var toolbarRoot = ( ViewGroup )toolbar?.Parent.Parent.Parent.Parent;

                if ( toolbarRoot == null ) return null;

                if ( toolbarRoot.ChildCount <= 1 ) return toolbar;

                var newToolbar = toolbarRoot.GetChildAt(toolbarRoot.ChildCount - 1)
                    .FindViewById(Resource.Id.toolbar) as Toolbar;

                return newToolbar ?? toolbar;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            var page = e.NewElement as PublicRepositoryPage;
            var viewModel = page?.BindingContext as PublicRepositoryPageViewModel;

            if ( GetToolbar == null ) return;

            if ( viewModel?.SubTitle != null )
                GetToolbar.Subtitle = viewModel.SubTitle;

            if ( page == null ) return;

            page.Appearing += async (sender, ev3) =>
            {
                if ( GetToolbar == null || viewModel == null ) return;

                GetToolbar.Subtitle = viewModel.SubTitle;

                if ( viewModel.AvatarUrl.Contains("?") )
                    viewModel.AvatarUrl = viewModel.AvatarUrl.Substring
                        (0, viewModel.AvatarUrl.IndexOf("?", StringComparison.Ordinal));

                await Task.Run(async () =>
                {
                    var url = new URL(viewModel.AvatarUrl);
                    var connection = url.OpenConnection();
                    var stream = connection.InputStream;
                    var logo = await Drawable.CreateFromStreamAsync(stream, viewModel.Title + "_avatar");

                    if ( !( logo is BitmapDrawable ) ) return;

                    var bitmappedLogo = logo as BitmapDrawable;
                    var density = Context.Resources.DisplayMetrics.Density;
                    var cornerRadius = 3 * density;
                    var avatarSize = GetMaxAvatarSize();
                    var image = Bitmap.CreateScaledBitmap(bitmappedLogo.Bitmap, avatarSize, avatarSize, false);
                    var finalLogo = new BitmapDrawable(Context.Resources, GetWithRoundedCorners(image, cornerRadius));

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if ( GetToolbar != null )
                            GetToolbar.Logo = finalLogo;
                    });

                });

            };

            page.Disappearing += (sender, ev3) =>
            {
                if ( GetToolbar != null )
                {
                    GetToolbar.Subtitle = null;
                    GetToolbar.Logo = null;
                }
            };

        }

        private int GetMaxAvatarSize()
        {
            var styledAttributes = Context.Theme.ObtainStyledAttributes(new[] { Android.Resource.Attribute.ActionBarSize });
            var actionBarSize = ( int )styledAttributes.GetDimension(0, 100);
            styledAttributes.Recycle();

            if ( actionBarSize - 35 > 0 )
                return actionBarSize - 35;

            return actionBarSize;
        }

        private Bitmap GetWithRoundedCorners(Bitmap original, float radius)
        {
            var width = original.Width;
            var height = original.Height;

            var paint = new Paint
            {
                AntiAlias = true,
                Color = Color.White
            };

            var clipped = Bitmap.CreateBitmap(width, height, Argb8888);
            var canvas = new Canvas(clipped);
            canvas.DrawRoundRect(new RectF(0, 0, width, height), radius, radius, paint);
            paint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.DstIn));

            var rounded = Bitmap.CreateBitmap(width, height, Argb8888);
            canvas = new Canvas(rounded);
            canvas.DrawBitmap(original, 0, 0, null);
            canvas.DrawBitmap(clipped, 0, 0, paint);

            original.Recycle();
            clipped.Recycle();

            return rounded;
        }
    }
}