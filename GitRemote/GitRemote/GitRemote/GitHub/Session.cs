namespace GitRemote.GitHub
{
    public class Session
    {
        public string Login { get; }

        private readonly string _token;
        private readonly string _privateFeedUrl;

        public Session(string login, string token)
        {
            Login = login;
            _token = token;
        }

        public Session(string login, string token, string privateFeedUrl)
        {
            Login = login;
            _token = token;
            _privateFeedUrl = privateFeedUrl;
        }

        public string GetToken()
        {
            return _token;
        }

        public string GetPrivateFeedUrl()
        {
            return _privateFeedUrl;
        }


    }
}
