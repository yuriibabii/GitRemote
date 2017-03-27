using GitRemote.Services;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class IssueModel : BindableBase
    {
        #region ElementsPropertiesDeclaretion

        public string PullRequestFontIcon => FontIconsService.Octicons.PullRequest;
        public double PullRequestOpacity => IsPullRequest ? 1.0 : 0;
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

        private bool _isPullRequest;

        public bool IsPullRequest
        {
            get { return _isPullRequest; }
            set { SetProperty(ref _isPullRequest, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _repository;

        public string Repository
        {
            get { return _repository; }
            set { SetProperty(ref _repository, value); }
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

        private string _nomer;

        public string Nomer
        {
            get { return _nomer; }
            set { SetProperty(ref _nomer, value); }
        }

        private ObservableCollection<Color> _labels = new ObservableCollection<Color>(new Color[7]);

        public ObservableCollection<Color> Labels
        {
            get { return _labels; }
            set { SetProperty(ref _labels, value); }
        }

        #endregion
    }

}
