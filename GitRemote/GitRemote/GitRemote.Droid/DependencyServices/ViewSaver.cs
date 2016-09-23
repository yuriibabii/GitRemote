using Android.Views;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using GitRemote.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ViewSaver))]
namespace GitRemote.Droid.DependencyServices
{
    public class ViewSaver : IViewSaver
    {
        private static View _loginView;
        private static View _passwordView;
        private static string _lastView = "LoginEntry";

        public static void SaveLoginView(View loginView)
        {
            _loginView = loginView;
        }

        public static void SavePasswordView(View passwordView)
        {
            _passwordView = passwordView;
        }

        public static View GetLoginView()
        {
            return _loginView;
        }

        public static View GetPasswordView()
        {
            return _passwordView;
        }

        public static void SetLastView(string viewName)
        {
            _lastView = viewName;
        }

        public static string GetLastView()
        {
            return _lastView;
        }
    }
}