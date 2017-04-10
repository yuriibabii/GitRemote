using System;
using Android.Support.Design.Widget;
using Android.Text;
using Android.Views;
using Android.Views.InputMethods;
using GitRemote.CustomClasses;
using GitRemote.Droid.DependencyServices;
using GitRemote.Droid.Renderers;
using GitRemote.ViewModels.Authentication;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using TextChangedEventArgs = Android.Text.TextChangedEventArgs;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(FloatingEntry), typeof(FloatingEntryRenderer))]

namespace GitRemote.Droid.Renderers
{
    public class FloatingEntryRenderer : Xamarin.Forms.Platform.Android.AppCompat.ViewRenderer<FloatingEntry, View>
    {
        private TextInputLayout _nativeView;

        private TextInputLayout NativeView => _nativeView ?? (_nativeView = InitializeNativeView());

        protected override void OnElementChanged(ElementChangedEventArgs<FloatingEntry> e)
        {
            base.OnElementChanged(e);

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

            if (Control == null) return;

            if (e.NewElement == null) return;

            if (e.NewElement.ClassId == "LoginEntry")
            {
                Control.Selected = true;
                Control.RequestFocusFromTouch();
            }
            else if (e.NewElement.ClassId == "PasswordEntry")
                SetSendButtonAction();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Entry.PlaceholderProperty.PropertyName) SetHintText();
            if (e.PropertyName == Entry.TextColorProperty.PropertyName) SetTextColor();
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName) SetBackgroundColor();
            if (e.PropertyName == Entry.IsPasswordProperty.PropertyName) SetIsPassword();
            if (e.PropertyName == Entry.TextProperty.PropertyName) SetText();
        }

        private void EditTextOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            Element.Text = textChangedEventArgs.Text.ToString();
            NativeView.EditText.SetSelection(Element.Text.Length); //Sets cursor to the end of entry
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
            if (Element.TextColor == Color.Default)
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

        /// <summary>
        /// If Action of our entry is Send than call method from PCL
        /// </summary>
        private void SetSendButtonAction()
        {
            NativeView.EditText.EditorAction += (sender, e) =>
            {
                if (e.ActionId == ImeAction.Send)
                {
                    if (((LoginingPageViewModel)Element.BindingContext).LogInCommand.CanExecute())
                        ((LoginingPageViewModel)Element.BindingContext).LogInCommand.Execute();
                }
                else
                    e.Handled = false;
            };
        }
    }

}