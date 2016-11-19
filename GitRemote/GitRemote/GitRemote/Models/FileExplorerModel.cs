using Prism.Mvvm;
using Xamarin.Forms;
using static GitRemote.Services.FontIconsService.Octicons;

namespace GitRemote.Models
{
    public class FileExplorerModel : BindableBase
    {
        public string FolderIcon => Directory;
        public string FileIcon => File;
        public bool IsFile => !IsFolder;
        public bool IsFolder => FileType == "dir";
        public FontAttributes IsBold => IsFolder ? FontAttributes.Bold : FontAttributes.None;
        public string FileTypeIcon => IsFolder ? FolderIcon : FileIcon;

        private string _name = string.Empty;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _fileType = string.Empty;

        public string FileType
        {
            get { return _fileType; }
            set { SetProperty(ref _fileType, value); }
        }

        private string _foldersCount;

        public string FoldersCount
        {
            get { return _foldersCount; }
            set { SetProperty(ref _foldersCount, value); }
        }

        private string _filesCount;

        public string FilesCount
        {
            get { return _filesCount; }
            set { SetProperty(ref _filesCount, value); }
        }

        private string _fileSize;

        public string FileSize
        {
            get { return _fileSize; }
            set { SetProperty(ref _fileSize, value); }
        }

        private GridLength _fileWidth;

        public GridLength FileWidth
        {
            get
            {
                FileWidth = IsFile ? GridLength.Auto : 0;
                return _fileWidth;
            }
            set { SetProperty(ref _fileWidth, value); }
        }

        private GridLength _folderWidth;

        public GridLength FolderWidth
        {
            get
            {
                FolderWidth = IsFolder ? GridLength.Auto : 0;
                return _folderWidth;
            }
            set { SetProperty(ref _folderWidth, value); }
        }


    }
}
