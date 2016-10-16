using Xamarin.Forms;

namespace GitRemote.Models
{
    public class PrivateNewsModel
    {
        public string Title { get; set; }
        public string Published { get; set; }
        public string ActionType { get; set; }
        public string ImageUrl { get; set; }
        public string Perfomer { get; set; }
        public string Target { get; set; }
        public string AdditionalTarget { get; set; }
        public string ActionTypeFontIcon { get; set; }

        public FormattedString CustomFormattedText => new FormattedString
        {
            Spans = {
                new Span { Text = Perfomer + " ", FontAttributes=FontAttributes.Bold, FontSize=16 },
                new Span { Text = ActionType + " ", FontSize=16 },
                new Span { Text = ActionType == "created" ? "reposiroty " : "", FontSize = 16},
                new Span { Text = ActionType == "added" ? AdditionalTarget + " " : "",
                    FontAttributes =FontAttributes.Bold, FontSize = 16},
                new Span { Text = ActionType == "added" ? "to " : "", FontSize = 16},
                new Span { Text = Target, FontAttributes=FontAttributes.Bold, FontSize = 16} }
        };

        public PrivateNewsModel() { }
    }
}
