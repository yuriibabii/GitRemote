using GitRemote.Services;
using Prism.Mvvm;

namespace GitRemote.Models
{
    public class RepositoryModel : BindableBase
    {
        #region Properties

        public bool IsDescription { get; set; }

        public string TypeIcon => Type == "Fork"
            ? FontIconsService.Octicons.RepoForked
            : (Type == "Private"
                ? FontIconsService.Octicons.Lock
                : FontIconsService.Octicons.Repo);

        private string _type;

        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _ownerName;

        public string OwnerName
        {
            get { return _ownerName; }
            set { SetProperty(ref _ownerName, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _language;

        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        private bool _isLanguage;

        public bool IsLanguage
        {
            get { return _isLanguage; }
            set { SetProperty(ref _isLanguage, value); }
        }

        private string _starIcon;

        public string StarIcon
        {
            get { return _starIcon; }
            set { SetProperty(ref _starIcon, value); }
        }

        private string _starsCount;

        public string StarsCount
        {
            get { return _starsCount; }
            set { SetProperty(ref _starsCount, value); }
        }

        private string _forkIcon;

        public string ForkIcon
        {
            get { return _forkIcon; }
            set { SetProperty(ref _forkIcon, value); }
        }

        private string _forksCount;

        public string ForksCount
        {
            get { return _forksCount; }
            set { SetProperty(ref _forksCount, value); }
        }
        #endregion
    }
}
