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

    }
}
