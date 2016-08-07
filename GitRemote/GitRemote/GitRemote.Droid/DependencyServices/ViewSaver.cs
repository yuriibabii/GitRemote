using Android.Views;
using GitRemote.Droid.DependencyServices;
using GitRemote.Services;

[assembly: Xamarin.Forms.Dependency(typeof(ViewSaver))]
namespace GitRemote.Droid.DependencyServices
{
    public class ViewSaver : IViewSaver
    {
        public static View LoginView;
        public static View PasswordView;
        private static string _lastView = string.Empty;

        public static void SaveLoginView(View loginView)
        {
            LoginView = loginView;
        }

        public static void SavePasswordView(View passwordView)
        {
            PasswordView = passwordView;
        }

        public static View GetLoginView()
        {
            return LoginView;
        }

        public static View GetPasswordView()
        {
            return PasswordView;
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