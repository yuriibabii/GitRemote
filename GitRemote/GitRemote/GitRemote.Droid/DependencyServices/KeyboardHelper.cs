using System.Threading.Tasks;
using Android.Content;
using Android.Views.InputMethods;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using GitRemote.Services;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(KeyboardHelper))]
namespace GitRemote.Droid.DependencyServices
{
    public class KeyboardHelper : Java.Lang.Object, IKeyboardHelper
    {
        private Context _context;
        private Android.Views.View _currentView;

        public void ShowKeyboard()
        {
            _currentView = ViewSaver.GetLastView() == "LoginEntry" ? ViewSaver.GetLoginView() : ViewSaver.GetPasswordView();
            _currentView.RequestFocus();
            _context = Forms.Context;
            var inputMethodManager = _context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager?.ShowSoftInput(_currentView, ShowFlags.Forced);

        }

        public void HideKeyboard()
        {
            _currentView = ViewSaver.GetLastView() == "LoginEntry" ? ViewSaver.GetLoginView() : ViewSaver.GetPasswordView();
            _currentView.ClearFocus();
            _context = Forms.Context;
            var inputMethodManager = _context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager?.HideSoftInputFromWindow(_currentView.WindowToken, HideSoftInputFlags.None);
        }


    }
}