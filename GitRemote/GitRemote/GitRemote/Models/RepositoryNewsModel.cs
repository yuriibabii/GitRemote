using System;
using Prism.Mvvm;
using Xamarin.Forms;

namespace GitRemote.Models
{
    public class RepositoryNewsModel : BindableBase
    {
        #region ElementsPropertiesDeclaretion

        private string _perfomer = string.Empty;

        public string Perfomer
        {
            get { return _perfomer; }
            set { SetProperty(ref _perfomer, value); }
        }

        private string _target = string.Empty;

        public string Target
        {
            get
            {
                switch ( EventType )
                {
                    case "PullRequestEvent":
                        Target = "pull request " + Nomer;
                        break;

                    case "IssuesEvent":
                    case "IssueCommentEvent":
                        Target = "issue " + Nomer;
                        break;

                    case "DeleteEvent":
                    case "CreateEvent":
                        if ( ActionType == "tag" )
                            Target = Nomer;
                        break;
                }

                return _target;
            }
            set { SetProperty(ref _target, value); }
        }

        private string _avatarImageUrl;

        public string AvatarImageUrl
        {
            get { return _avatarImageUrl; }
            set { SetProperty(ref _avatarImageUrl, value); }
        }

        private string _commitsCount = "0";

        public string CommitsCount
        {
            get
            {
                return _commitsCount == "1"
                    ? _commitsCount + " new commit"
                    : _commitsCount + " new commits";
            }
            set { SetProperty(ref _commitsCount, value); }
        }

        private string _shaCode = string.Empty;

        public string ShaCode
        {
            get { return _shaCode; }
            set { SetProperty(ref _shaCode, value); }
        }

        private string _comment = string.Empty;

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

        private string _publishedTime = string.Empty;

        public string PublishedTime
        {
            get { return _publishedTime; }
            set { SetProperty(ref _publishedTime, value); }
        }

        private string _nomer = string.Empty;

        public string Nomer
        {
            get { return _nomer; }
            set { SetProperty(ref _nomer, value); }
        }

        #region Body

        private string _body;

        public string Body
        {
            get
            {
                if ( EventType == "PushEvent" )
                    Body = ShaCode + ' ' + Comment;
                return _body;
            }
            set { SetProperty(ref _body, value); }
        }

        private bool _isBody;

        public bool IsBody
        {
            get { return _isBody; }
            set { SetProperty(ref _isBody, value); }
        }

        private GridLength _bodyHeight;

        public GridLength BodyHeight
        {
            get
            {
                BodyHeight = IsBody ? 20 : 0;
                return _bodyHeight;
            }
            set { SetProperty(ref _bodyHeight, value); }
        }
        #endregion

        #region Subtitle

        private string _subtitle;

        public string Subtitle
        {
            get
            {
                if ( EventType == "PushEvent" )
                    Subtitle = CommitsCount;
                if ( EventType == "CommitCommentEvent" )
                    Subtitle = "Comment in " + ShaCode;
                return _subtitle;
            }
            set { SetProperty(ref _subtitle, value); }
        }

        private bool _isSubtitle;

        public bool IsSubtitle
        {
            get { return _isSubtitle; }
            set { SetProperty(ref _isSubtitle, value); }
        }

        private GridLength _subtitleHeight;

        public GridLength SubtitleHeight
        {
            get
            {
                SubtitleHeight = IsSubtitle ? 20 : 0;
                return _subtitleHeight;
            }
            set { SetProperty(ref _subtitleHeight, value); }
        }
        #endregion






        public FormattedString Title => new FormattedString
        {
            Spans =
            {
                new Span {Text = Perfomer + " ", FontAttributes = FontAttributes.Bold, FontSize = 15},
                new Span {Text = Action + " ", FontSize = 15},
                new Span {Text = Target + " ", FontAttributes = FontAttributes.Bold, FontSize = 15},
                new Span {Text = Tail, FontSize = 15}
            }
        };

        private string Action
        {
            get
            {
                switch ( EventType )
                {
                    case "PushEvent":
                        return ActionType + " to";

                    case "PullRequestEvent":
                        return ActionType;

                    case "IssuesEvent":
                        return ActionType;

                    case "IssueCommentEvent":
                        return "commented on";

                    case "CommitCommentEvent":
                        return "commented";

                    case "CreateEvent":
                        return string.Concat("created ", ActionType == "branch" ? "branch"
                            : ( ActionType == "tag" ? "tag"
                            : "repository" ));

                    case "DeleteEvent":
                        return string.Concat("deleted ", ActionType == "tag" ? "tag" : "branch");

                    case "GollumEvent":
                        return "updated the wiki";

                    case "MemberEvent":
                        return ActionType;

                    case "PublicEvent":
                        return "made repository public";

                    case "ReleaseEvent":
                        return "released";

                    case "WatchEvent":
                        return "starred repository";

                    default:
                        return "";
                }
            }
        }

        private string Tail => EventType == "MemberEvent" ? "as a collaborator" : string.Empty;

        #endregion
    }
}
