using GitRemote.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace GitRemote.GitHub
{
    public class AccountManager
    {
        private readonly ClientAuthorization _clientAuthorization;
        private readonly ISecuredDataProvider _securedDataProvider;
        private static string _lastUser = GetLastUserFromStorage();
        private readonly List<string> _users;

        public AccountManager(ClientAuthorization clientAuthorization, ISecuredDataProvider securedDataProvider)
        {
            _clientAuthorization = clientAuthorization;
            _securedDataProvider = securedDataProvider;
            _users = _securedDataProvider.RetreiveAll(ConstantsService.ProviderName).Select(acc => acc.Username) as List<string>;
        }

        public void AddAccount(string login, string password)
        {
            var token = GetToken(login, password);

            CheckForExist(login);

            _securedDataProvider.Store(login, ConstantsService.ProviderName,
                new Dictionary<string, string> { { _clientAuthorization.GetNote(), token } });

            SetLastUser(login);
            _users.Insert(0, login);
        }

        private string GetToken(string login, string password)
        {
            var token = _clientAuthorization.GenerateToken(new Client(login, password).GetClient());

            if ( token == null )
                throw new Exception("Something wrong with generating token");

            return token;
        }

        private void CheckForExist(string login)
        {
            var retreiveResponce = _securedDataProvider.Retreive(ConstantsService.ProviderName, login).Username;

            if ( retreiveResponce == null ) return;

            _securedDataProvider.Clear(retreiveResponce);

            if ( _users.Contains(login) )
                _users.Remove(login);
        }

        #region LastUser
        public static void SetLastUser(string userName)
        {
            _lastUser = userName;
        }

        public static void SaveLastUser()
        {
            Application.Current.Properties["_lastUser"] = _lastUser;
        }

        public string GetLastUser()
        {
            return _lastUser;
        }

        public static string GetLastUserFromStorage()
        {

            if ( Application.Current.Properties.ContainsKey("_lastUser") )
                return Application.Current.Properties["_lastUser"] as string;

            return string.Empty;
        }
        #endregion
    }
}
