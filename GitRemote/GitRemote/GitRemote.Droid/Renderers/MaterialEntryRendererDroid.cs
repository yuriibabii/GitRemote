using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using GitRemote.Droid.Renderers;
using GitRemote.Views;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(MaterialEntry), typeof(MaterialEntryRendererDroid))]

namespace GitRemote.Droid.Renderers
{
    public class MaterialEntryRendererDroid : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<Entry, View>
    {
        private TextInputLayout _nativeView;

        private TextInputLayout NativeView => _nativeView ?? ( _nativeView = InitializeNativeView() );

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);



            if ( e.OldElement != null ) return;

            // MaterialEntry Render Staff
            #region
            var ctrl = CreateNativeControl();
            SetNativeControl(ctrl);

            SetText();
            SetHintText();
            SetBackgroundColor();
            SetTextColor();
            SetIsPassword();
            #endregion

            if ( ( Control != null ) && ( e.NewElement.ClassId == "PasswordEntry" ) )
                SetDoneButtonAction();
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
            return view;
        }

        protected override View CreateNativeControl()
        {
            return LayoutInflater.From(Context).Inflate(Resource.Layout.TextInputLayout, null);
        }

        private void SetDoneButtonAction()
        {
            //var control = (EditText) Control;
            //control.ImeOptions = ImeAction.Done;
            //control.EditorAction += (sender, args) =>
            //{
            //    if (args.ActionId == ImeAction.Done)
            //    {
            //        var entry = (MaterialEntry) Element;
            //        entry.DonePressed();
            //    }
            //};

            NativeView.EditText.EditorAction += (sender, e) =>
            {
                if ( e.ActionId == ImeAction.Done )
                { Control.}
            }
        }
    }

}