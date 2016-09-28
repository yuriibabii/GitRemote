using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using GitRemote.CustomClasses;
using GitRemote.Droid.DependencyServices;
using GitRemote.Droid.Renderers;
using GitRemote.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(FloatingEntry), typeof(FloatingEntryRenderer))]

namespace GitRemote.Droid.Renderers
{
    public class FloatingEntryRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, View>
    {
        private TextInputLayout _nativeView;

        private TextInputLayout NativeView => _nativeView ?? ( _nativeView = InitializeNativeView() );

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if ( e.OldElement != null ) return;

            // FloatingEntry Render Staff
            #region
            var ctrl = CreateNativeControl();
            SetNativeControl(ctrl);

            SetText();
            SetHintText();
            SetBackgroundColor();
            SetTextColor();
            SetIsPassword();
            #endregion

            if ( Control == null ) return;

            switch ( e.NewElement.ClassId )
            {
                case "LoginEntry":
                    ViewSaver.SaveLoginView(Control);
                    break;
                case "PasswordEntry":
                    SetSendButtonAction();
                    ViewSaver.SavePasswordView(Control);
                    break;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if ( e.PropertyName == Entry.PlaceholderProperty.PropertyName )
            {
                SetHintText();
            }

            if ( e.PropertyName == Entry.TextColorProperty.PropertyName )
            {
                SetTextColor();
            }

            if ( e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName )
            {
                SetBackgroundColor();
            }

            if ( e.PropertyName == Entry.IsPasswordProperty.PropertyName )
            {
                SetIsPassword();
            }

            if ( e.PropertyName == Entry.TextProperty.PropertyName )
            {
                SetText();
            }

        }

        private void EditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Element.Text = textChangedEventArgs.Text.ToString();
            NativeView.EditText.SetSelection(Element.Text.Length);
        }

        private void EditTextOnFocusChanged(object sender, FocusChangeEventArgs focusChangedEventArgs)
        {
            if ( focusChangedEventArgs.HasFocus )
                ViewSaver.SetLastView(Element.ClassId);
        }

        private void SetText()
        {
            NativeView.EditText.Text = Element.Text;
        }

        private void SetIsPassword()
        {
            NativeView.EditText.InputType = Element.IsPassword
                ? InputTypes.TextVariationPassword | InputTypes.ClassText
                : InputTypes.TextVariationVisiblePassword;
        }

        public void SetBackgroundColor()
        {
            NativeView.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
        }

        private void SetHintText()
        {
            NativeView.Hint = Element.Placeholder;
        }

        private void SetTextColor()
        {
            if ( Element.TextColor == Color.Default )
                NativeView.EditText.SetTextColor(NativeView.EditText.TextColors);
            else
                NativeView.EditText.SetTextColor(Element.TextColor.ToAndroid());
        }

        private TextInputLayout InitializeNativeView()
        {
            var view = FindViewById<TextInputLayout>(Resource.Id.textInputLayout);
            view.EditText.TextChanged += EditTextOnTextChanged;
            view.EditText.FocusChange += EditTextOnFocusChanged;
            return view;
        }

        protected override View CreateNativeControl()
        {
            return LayoutInflater.From(Context).Inflate(Resource.Layout.TextInputLayout, null);
        }

        /// <summary>
        /// If Action of our entry is Send than call method from Portable
        /// </summary>
        private void SetSendButtonAction()
        {
            NativeView.EditText.EditorAction += (sender, e) =>
            {
                if ( e.ActionId == ImeAction.Send )
                {
                    ( ( LoginingPageViewModel )Element.BindingContext ).OnLogInTapped();
                }
                else
                    e.Handled = false;
            };
        }


    }

}