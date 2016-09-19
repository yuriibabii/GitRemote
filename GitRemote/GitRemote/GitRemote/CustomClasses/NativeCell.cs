using Xamarin.Forms;

namespace GitRemote.CustomClasses
{
    public class NativeCell : TextCell
    {
        public static BindableProperty UserNameProperty =
            BindableProperty.Create("UserName", typeof(string), typeof(NativeCell), "");

        public string UserName
        {
            get { return ( string )GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }

        public static BindableProperty ChangeBackgroundWhenSelectedProperty =
            BindableProperty.Create("ChangeBackgroundWhenSelected", typeof(bool), typeof(NativeCell), false);

        public bool ChangeBackgroundWhenSelected
        {
            get { return ( bool )GetValue(ChangeBackgroundWhenSelectedProperty); }
            set { SetValue(ChangeBackgroundWhenSelectedProperty, value); }
        }

        public static BindableProperty BackgroundColorWhenSelectedProperty =
            BindableProperty.Create("BackgroundColorWhenSelected", typeof(Color), typeof(NativeCell), Color.Accent);

        public Color BackgroundColorWhenSelected
        {
            get { return ( Color )GetValue(BackgroundColorWhenSelectedProperty); }
            set { SetValue(BackgroundColorWhenSelectedProperty, value); }
        }
    }
}
