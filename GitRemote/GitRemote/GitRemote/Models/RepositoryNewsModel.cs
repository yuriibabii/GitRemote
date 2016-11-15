using Prism.Mvvm;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class RepositoryNewsModel : BindableBase
    {
        #region ElementsPropertiesDeclaretion

        private string _perfomer;

        public string Perfomer
        {
            get { return _perfomer; }
            set { SetProperty(ref _perfomer, value); }
        }

        private string _target;

        public string Target
        {
            get { return _target; }
            set { SetProperty(ref _target, value); }
        }

        private string _avatarImageUrl;

        public string AvatarImageUrl
        {
            get { return _avatarImageUrl; }
            set { SetProperty(ref _avatarImageUrl, value); }
        }

        private int _commitsCount;

        public int CommitsCount
        {
            get { return _commitsCount; }
            set { SetProperty(ref _commitsCount, value); }
        }

        private bool _isCommits;

        public bool IsCommits
        {
            get { return _isCommits; }
            set { SetProperty(ref _isCommits, value); }
        }

        private string _payloadHead;

        public string PayloadHead
        {
            get { return _payloadHead; }
            set { SetProperty(ref _payloadHead, value); }
        }

        private bool _isPayloadHead;

        public bool IsPayloadHead
        {
            get { return _isPayloadHead; }
            set { SetProperty(ref _isPayloadHead, value); }
        }

        private bool _isComment;

        public bool IsComment
        {
            get { return _isComment; }
            set { SetProperty(ref _isComment, value); }
        }

        private string _comment;

        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        private string _eventType;

        public string EventType
        {
            get { return _eventType; }
            set { SetProperty(ref _eventType, value); }
        }

        private string _actionType;

        public string ActionType
        {
            get { return _actionType; }
            set { SetProperty(ref _actionType, value); }
        }

        private string _actionTypeFontIcon;

        public string ActionTypeFontIcon
        {
            get { return _actionTypeFontIcon; }
            set { SetProperty(ref _actionTypeFontIcon, value); }
        }

        private string _publishedTime;

        public string PublishedTime
        {
            get { return _publishedTime; }
            set { SetProperty(ref _publishedTime, value); }
        }

        private int _nomer;

        public int Nomer
        {
            get { return _nomer; }
            set { SetProperty(ref _nomer, value); }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _isTitle;

        public bool IsTitle
        {
            get { return _isTitle; }
            set { SetProperty(ref _isTitle, value); }
        }

        public FormattedString CustomFormattedText => new FormattedString
        {
            //Spans = {
            //    new Span { Text = Perfomer + " ", FontAttributes=FontAttributes.Bold, FontSize=16 },
            //    new Span { Text = ActionType !="opened" ? ActionType + " " : "opened issue ", FontSize=16 },
            //    new Span { Text = ActionType == "created" ? "reposiroty " : "", FontSize = 16},
            //    new Span { Text = ActionType == "added" ? AdditionalTarget + " " : "",
            //        FontAttributes =FontAttributes.Bold, FontSize = 16},
            //    new Span { Text = ActionType == "added" ? "to " : "", FontSize = 16},
            //    new Span { Text = , FontAttributes=FontAttributes.Bold, FontSize = 16} }
        };

        #endregion
    }
}
