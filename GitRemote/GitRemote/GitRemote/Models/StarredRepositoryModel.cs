using GitRemote.Services;
using Prism.Mvvm;
using Xamarin.Forms;
using static GitRemote.Common.Enums;
using static GitRemote.Services.FontIconsService.Octicons;

namespace GitRemote.Models
{
    public class StarredRepositoryModel : BindableBase
    {
        public FormattedString FormattedName => new FormattedString
        {
            Spans =
            {
                new Span { Text = Path},
                new Span { Text = Name, FontAttributes = FontAttributes.Bold }
            }
        };

        public bool IsDescription { get; set; }

        public string TypeIcon => Type == RepositoriesTypes.Fork
             ? RepoForked
             : (Type == RepositoriesTypes.Private ? Lock : Repo);

        public RepositoriesTypes Type { get; set; }
        public string OwnerName { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public bool IsLanguage { get; set; }
        public string StarsCount { get; set; }
        public string ForksCount { get; set; }
    }
}
