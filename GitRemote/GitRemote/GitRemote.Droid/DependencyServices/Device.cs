using Android.Content;
using Android.Util;
using GitRemote.DI;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Device = GitRemote.Droid.DependencyServices.Device;

[assembly: Dependency(typeof(Device))]
namespace GitRemote.Droid.DependencyServices
{
    public class Device : IDevice
    {
        public Task<bool> LaunchUriAsync(Uri uri)
        {
            var launchTaskSource = new TaskCompletionSource<bool>();
            try
            {
                Forms.Context.StartActivity(new Intent("android.intent.action.VIEW", Android.Net.Uri.Parse(uri.ToString())));
                launchTaskSource.SetResult(true);
            }
            catch ( Exception ex )
            {
                Log.Error("Device.LaunchUriAsync", ex.Message);
                launchTaskSource.SetException(ex);
            }

            return launchTaskSource.Task;
        }
    }
}