using Android.Content;
using Android.Views.InputMethods;
using GitRemote.DI;
using GitRemote.Droid.DependencyServices;
using Xamarin.Forms;

[assembly: Dependency(typeof(KeyboardHelper))]
namespace GitRemote.Droid.DependencyServices
{
    public class KeyboardHelper : Java.Lang.Object, IKeyboardHelper
    {
        public void ShowKeyboard()
        {
            var inputMethodManager = Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager?.ToggleSoftInput(ShowFlags.Implicit, 0);
        }

        public void HideKeyboard()
        {
            var inputMethodManager = Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            inputMethodManager?.ToggleSoftInput(0, HideSoftInputFlags.ImplicitOnly);
        }

        public bool IsKeyboard()
        {
            var inputMethodManager = Forms.Context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            return inputMethodManager?.IsAcceptingText ?? false;
        }
    }
}