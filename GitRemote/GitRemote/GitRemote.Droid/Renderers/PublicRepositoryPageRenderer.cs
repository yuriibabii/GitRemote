using System;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.Content;
using Android.Views;
using GitRemote.Droid.Renderers;
using GitRemote.ViewModels;
using GitRemote.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
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

                if ( toolbar == null )
                {
                    return null;
                }

                var toolbarRoot = ( ViewGroup )toolbar.Parent.Parent.Parent.Parent;

                if ( toolbarRoot?.ChildCount > 1 )
                {
                    var newToolbar = toolbarRoot.GetChildAt(toolbarRoot.ChildCount - 1).FindViewById(Resource.Id.toolbar) as Toolbar;

                    if ( newToolbar != null )
                    {
                        return newToolbar;
                    }
                }

                return toolbar;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);
            var page = e.NewElement as PublicRepositoryPage;
            var viewModel = page?.BindingContext as PublicRepositoryPageViewModel;

            if ( GetToolbar != null )
            {
                if ( viewModel?.SubTitle != null )
                {
                    GetToolbar.Subtitle = viewModel.SubTitle;
                }

                if ( page != null )
                {
                    page.Appearing += (sender, ev3) =>
                    {
                        if (GetToolbar == null) return;

                        GetToolbar.Subtitle = viewModel?.SubTitle;
                        var stream = Context.ContentResolver.OpenInputStream(Android.Net.Uri.Parse("file://" + viewModel?.AvatarUrl));

                        var ident = Drawable.CreateFromStream(stream, viewModel?.AvatarUrl);

                        GetToolbar.NavigationIcon = ident;
                    };

                    page.Disappearing += (sender, ev3) =>
                    {
                        if ( GetToolbar != null )
                        {
                            GetToolbar.Subtitle = null;
                        }
                    };
                }
            }
        }
    }
}