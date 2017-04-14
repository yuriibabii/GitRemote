using GitRemote.DI;
using GitRemote.Services;
using Xamarin.Forms;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.Models
{
    public class PrivateNewsModel
    {
        public enum ActionTypes
        {
            Opened,
            Created,
            Added,
            Forked,
            Starred,
            Made
        }

        public string Title { get; set; }
        public string Date { get; set; }
        public ActionTypes ActionType { get; set; }
        public string AvatarUrl { get; set; }
        public string Perfomer { get; set; }
        public string Target { get; set; }
        public string AdditionalTarget { get; set; }
        public string ActionTypeFontIcon { get; set; }

        public FormattedString CustomFormattedText => new FormattedString
        {
            Spans =
            {
                new Span {Text = Perfomer, FontAttributes = FontAttributes.Bold},
                new Span
                {
                    Text = ActionType != ActionTypes.Opened
                        ? Space + ActionType.ToString().ToLower()
                        : Space + "opened issue"
                },
                new Span
                {
                    Text = ActionType == ActionTypes.Created
                        ? Space + "repository"
                        : Empty
                },
                new Span
                {
                    Text = ActionType == ActionTypes.Added
                        ? Space + AdditionalTarget
                        : Empty,
                    FontAttributes = FontAttributes.Bold
                },
                new Span
                {
                    Text = ActionType == ActionTypes.Added
                        ? Space + "to"
                        : Empty
                },
                new Span {Text = Space + Target, FontAttributes = FontAttributes.Bold},
                new Span
                {
                    Text = ActionType == ActionTypes.Made
                        ? Space + "public"
                        : Empty
                }
            }
        };
    }
}
