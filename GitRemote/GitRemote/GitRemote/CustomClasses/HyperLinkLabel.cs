using Xamarin.Forms;

namespace GitRemote.CustomClasses
{
    public class HyperLinkLabel : Label
    {
        public static readonly BindableProperty IsUnderlineProperty =
            BindableProperty.Create("IsUnderline", typeof(bool), typeof(HyperLinkLabel), true);

        public bool IsUnderline
        {
            get { return ( bool )GetValue(IsUnderlineProperty); }
            set { SetValue(IsUnderlineProperty, value); }
        }
    }
}