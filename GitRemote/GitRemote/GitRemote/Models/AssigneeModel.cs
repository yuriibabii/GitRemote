using Prism.Mvvm;

namespace GitRemote.Models
{
    public class AssigneeModel : BindableBase
    {
        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _avatarUrl = string.Empty;
        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { SetProperty(ref _avatarUrl, value); }
        }

        private bool _isActivated;

        public bool IsActivated
        {
            get { return _isActivated; }
            set { SetProperty(ref _isActivated, value); }
        }
    }
}
