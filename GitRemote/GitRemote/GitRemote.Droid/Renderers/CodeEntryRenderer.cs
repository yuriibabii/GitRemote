using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using GitRemote.CustomClasses;
using GitRemote.Droid.Renderers;
using GitRemote.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CodeEntry), typeof(CodeEntryRenderer))]

namespace GitRemote.Droid.Renderers
{
    public class CodeEntryRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if ( e.OldElement != null ) return;

            var ctrl = CreateNativeControl();
            ctrl.RequestFocus(); // Gives focus to CodeEntry
            var editText = ( EditText )( ( LinearLayout )ctrl ).GetChildAt(0); //Gets EditText element from layout
            editText.TextChanged += CodeTextOnTextChanged;
            editText.EditorAction += AttachEditorAction;
            SetNativeControl(ctrl);
        }

        private void CodeTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Element.Text = textChangedEventArgs.Text.ToString();
        }

        private void AttachEditorAction(object sender, TextView.EditorActionEventArgs e)
        {
            if ( e.ActionId == ImeAction.Done )
            {
                if ( ( ( TwoFactorAuthPageViewModel )Element.BindingContext ).LogInCommand.CanExecute() )
                    ( ( TwoFactorAuthPageViewModel )Element.BindingContext ).LogInCommand.Execute();
            }
            else
                e.Handled = false;
        }

        /// <summary>
        /// Gets view from layout in xml
        /// </summary>
        /// <returns></returns>
        protected override View CreateNativeControl()
        {
            return LayoutInflater.From(Context).Inflate(Resource.Layout.CodeInputLayout, null);
        }
    }
}