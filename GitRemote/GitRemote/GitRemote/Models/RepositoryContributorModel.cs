using Prism.Mvvm;
using static System.String;

namespace GitRemote.Models
{
    public class RepositoryContributorModel : BindableBase
    {
        private string _login = Empty;

        public string Login
        {
            get { return _login; }
            set { SetProperty(ref _login, value); }
        }

        private string _avatarUrl = Empty;

        public string AvatarUrl
        {
            get { return _avatarUrl; }
            set { SetProperty(ref _avatarUrl, value); }
        }

        public int CommitsCount { get; set; }

        public string CommitsText => CommitsCount + " commits";
    }
}
