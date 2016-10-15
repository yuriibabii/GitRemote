namespace GitRemote.Models
{
    public class PrivateNewsModel
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public string Description { get; set; }
        public PrivateNewsModel(string title, string link, string description)
        {
            Title = title;
            Link = link;
            Description = description;
        }

        public PrivateNewsModel() { }
    }
}
