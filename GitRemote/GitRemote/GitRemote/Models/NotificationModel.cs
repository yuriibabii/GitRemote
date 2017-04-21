using Prism.Mvvm;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class NotificationModel : BindableBase
    {
        private bool _isRead;
        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                SetProperty(ref _isRead, value);

                if ( IsRead )
                {
                    TitleColor = Color.FromHex("054678");
                    TypeIconColor = Color.FromHex("054678");
                    FontAttr = FontAttributes.None;
                }
                else
                {
                    TitleColor = Color.Black;
                    TypeIconColor = Color.Black;
                    FontAttr = FontAttributes.Bold;
                }
            }
        }

        private string _typeIcon;
        public string TypeIcon
        {
            get { return _typeIcon; }
            set { SetProperty(ref _typeIcon, value); }
        }

        private string _fullName;
        public string FullName
        {
            get { return _fullName; }
            set { SetProperty(ref _fullName, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { SetProperty(ref _time, value); }
        }

        private Color _titleColor;
        public Color TitleColor
        {
            get { return _titleColor; }
            set { SetProperty(ref _titleColor, value); }
        }

        private Color _typeIconColor;
        public Color TypeIconColor
        {
            get { return _typeIconColor; }
            set { SetProperty(ref _typeIconColor, value); }
        }

        private FontAttributes _fontAttr;
        public FontAttributes FontAttr
        {
            get { return _fontAttr; }
            set { SetProperty(ref _fontAttr, value); }
        }
    }
}
