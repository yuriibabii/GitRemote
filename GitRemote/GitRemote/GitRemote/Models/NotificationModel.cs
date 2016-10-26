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
                NotifyTitleColor = IsRead ? Color.Gray : Color.Black;
                NotifyTypeIconColor = IsRead ? Color.Gray : Color.FromHex("054678");
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

        #endregion

    }
}
