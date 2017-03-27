using GitRemote.Services;
using Prism.Mvvm;

namespace GitRemote.Models
{
    public class RepositoryModel : BindableBase
    {
        #region ElementsPropertiesDeclaretion

        public bool IsDescription { get; set; }

        public string RepositoryTypeIcon => RepositoryType == "Fork"
            ? FontIconsService.Octicons.RepoForked
            : ( RepositoryType == "Private"
                ? FontIconsService.Octicons.Lock
                : FontIconsService.Octicons.Repo );

        private string _repositoryType;

        public string RepositoryType
        {
            get { return _repositoryType; }
            set { SetProperty(ref _repositoryType, value); }
        }

        private string _repositoryName;

        public string RepositoryName
        {
            get { return _repositoryName; }
            set { SetProperty(ref _repositoryName, value); }
        }

        private string _ownerName;

        public string OwnerName
        {
            get { return _ownerName; }
            set { SetProperty(ref _ownerName, value); }
        }

        private string _repositoryDescription;

        public string RepositoryDescription
        {
            get { return _repositoryDescription; }
            set { SetProperty(ref _repositoryDescription, value); }
        }

        private string _repositoryLanguage;

        public string RepositoryLanguage
        {
            get { return _repositoryLanguage; }
            set { SetProperty(ref _repositoryLanguage, value); }
        }

        private string _repositoryStarIcon;

        public string RepositoryStarIcon
        {
            get { return _repositoryStarIcon; }
            set { SetProperty(ref _repositoryStarIcon, value); }
        }

        private string _repositoryStarsCount;

        public string RepositoryStarsCount
        {
            get { return _repositoryStarsCount; }
            set { SetProperty(ref _repositoryStarsCount, value); }
        }

        private string _repositoryForkIcon;

        public string RepositoryForkIcon
        {
            get { return _repositoryForkIcon; }
            set { SetProperty(ref _repositoryForkIcon, value); }
        }

        private string _repositoryForksCount;

        public string RepositoryForksCount
        {
            get { return _repositoryForksCount; }
            set { SetProperty(ref _repositoryForksCount, value); }
        }
        #endregion


    }
}
