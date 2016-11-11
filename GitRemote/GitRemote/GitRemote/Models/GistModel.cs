using GitRemote.Services;
using Prism.Mvvm;

namespace GitRemote.Models
{
    public class GistModel : BindableBase
    {
        #region ElementsPropertiesDeclaretion

        public string FileFontIcon => FontIconsService.Octicons.File;
        public string CommentFontIcon => FontIconsService.Octicons.Comment;
        public double CommentOpacity => IsCommented ? 1.0 : 0.25;
        private bool _isCommented;

        public bool IsCommented
        {
            get { return _isCommented; }
            set { SetProperty(ref _isCommented, value); }
        }

        private int _commentsCount;

        public int CommentsCount
        {
            get { return _commentsCount; }
            set { SetProperty(ref _commentsCount, value); }
        }

        private int _filesCount;

        public int FilesCount
        {
            get { return _filesCount; }
            set { SetProperty(ref _filesCount, value); }
        }

        private string _description;

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private string _id;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _ownerName;

        public string OwnerName
        {
            get { return _ownerName; }
            set { SetProperty(ref _ownerName, value); }
        }

        private string _imageUrl;

        public string ImageUrl
        {
            get { return _imageUrl; }
            set { SetProperty(ref _imageUrl, value); }
        }

        private string _createdTime;

        public string CreatedTime
        {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }

        #endregion
    }
}
