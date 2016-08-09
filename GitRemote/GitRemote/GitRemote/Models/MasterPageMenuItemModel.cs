using Prism.Mvvm;

namespace GitRemote.Models
{
    public class MasterPageMenuItemModel : BindableBase
    {
        public string Name { get { return _name; } set { SetProperty(ref _name, value); } }
        public string ImageSource { get { return _name; } set { SetProperty(ref _imageSource, value); } }

        private string _name = string.Empty;
        private string _imageSource = string.Empty;

    }
}
