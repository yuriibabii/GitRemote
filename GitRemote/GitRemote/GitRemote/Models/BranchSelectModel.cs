using GitRemote.Services;
using Prism.Mvvm;

namespace GitRemote.Models
{
    public class BranchSelectModel : BindableBase
    {
        public string SelectTypeIcon => Type == "Branch"
            ? FontIconsService.Octicons.Branch
            : FontIconsService.Octicons.Tag;

        private string _type = string.Empty;

        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private bool _isActivated;

        public bool IsActivated
        {
            get { return _isActivated; }
            set { SetProperty(ref _isActivated, value); }
        }
    }
}
