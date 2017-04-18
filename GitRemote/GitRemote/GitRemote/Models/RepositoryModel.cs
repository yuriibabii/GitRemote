using GitRemote.Services;
using Prism.Mvvm;
using static GitRemote.Common.Enums;

namespace GitRemote.Models
{
    public class RepositoryModel : BindableBase
    {
        #region Properties

        public bool IsDescription { get; set; }

        public string TypeIcon => Type == RepositoriesTypes.Fork
            ? FontIconsService.Octicons.RepoForked
            : (Type == RepositoriesTypes.Private
                ? FontIconsService.Octicons.Lock
                : FontIconsService.Octicons.Repo);

        public RepositoriesTypes Type { get; set; }
        public string Name { get; set; }
        public string OwnerName { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public bool IsLanguage { get; set; }
        public string StarIcon { get; set; }
        public string StarsCount { get; set; }
        public string ForkIcon { get; set; }
        public string ForksCount { get; set; }

        #endregion
    }
}
