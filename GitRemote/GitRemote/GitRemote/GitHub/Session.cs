namespace GitRemote.GitHub
{
    public class Session
    {
        public string Login { get; }

        private readonly string _token;

        public Session(string login, string token)
        {
            Login = login;
            _token = token;
        }

        public string GetToken()
        {
            return _token;
        }


    }
}
