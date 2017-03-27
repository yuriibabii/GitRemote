using GitRemote.Services;
using Prism.Mvvm;

namespace GitRemote.Models
{
    public class CommitModel : BindableBase
    {
        private string _title = string.Empty;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _createdTime = string.Empty;

        public string CreatedTime
        {
            get { return _createdTime; }
            set { SetProperty(ref _createdTime, value); }
        }

        private string _avatarImageUrl = string.Empty;

        public string AvatarImageUrl
        {
            get { return _avatarImageUrl; }
            set { SetProperty(ref _avatarImageUrl, value); }
        }
        private string _id = string.Empty;

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _ownerName = string.Empty;

        public string OwnerName
        {
            get { return _ownerName; }
            set { SetProperty(ref _ownerName, value); }
        }

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
    }
}
