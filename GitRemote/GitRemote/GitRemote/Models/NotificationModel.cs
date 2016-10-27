using Prism.Mvvm;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class NotificationModel : BindableBase
    {

        #region ElementsPropertiesDeclaretion

        private bool _isRead;

        public bool IsRead
        {
            get { return _isRead; }
            set
            {
                SetProperty(ref _isRead, value);

                if ( IsRead )
                {
                    NotifyTitleColor = Color.FromHex("054678");
                    NotifyTypeIconColor = Color.FromHex("054678");
                    NotifyFontAttr = FontAttributes.None;
                }
                else
                {
                    NotifyTitleColor = Color.Black;
                    NotifyTypeIconColor = Color.Black;
                    NotifyFontAttr = FontAttributes.Bold;
                }
            }
        }

        private string _notifyTypeIcon;

        public string NotifyTypeIcon
        {
            get { return _notifyTypeIcon; }
            set { SetProperty(ref _notifyTypeIcon, value); }
        }

        private string _notifyFullName;

        public string NotifyFullName
        {
            get { return _notifyFullName; }
            set { SetProperty(ref _notifyFullName, value); }
        }

        private string _notifyTitle;

        public string NotifyTitle
        {
            get { return _notifyTitle; }
            set { SetProperty(ref _notifyTitle, value); }
        }

        private string _notifyTime;

        public string NotifyTime
        {
            get { return _notifyTime; }
            set { SetProperty(ref _notifyTime, value); }
        }

        private Color _notifyTitleColor;

        public Color NotifyTitleColor
        {
            get { return _notifyTitleColor; }
            set { SetProperty(ref _notifyTitleColor, value); }
        }

        private Color _notifyTypeIconColor;

        public Color NotifyTypeIconColor
        {
            get { return _notifyTypeIconColor; }
            set { SetProperty(ref _notifyTypeIconColor, value); }
        }

        private FontAttributes _notifyFontAttr;

        public FontAttributes NotifyFontAttr
        {
            get { return _notifyFontAttr; }
            set { SetProperty(ref _notifyFontAttr, value); }
        }

        #endregion
    }
}
