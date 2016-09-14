using GitRemote.Services;
using System;
using System.Collections.Generic;

namespace GitRemote.GitHub
{
    public class AccountManager
    {
        private readonly ClientAuthorization _clientAuthorization;
        private readonly ISecuredDataProvider _securedDataProvider;

        public AccountManager(ClientAuthorization clientAuthorization, ISecuredDataProvider securedDataProvider)
        {
            _clientAuthorization = clientAuthorization;
            _securedDataProvider = securedDataProvider;
        }

        public void AddAccount(string login, string password)
        {
            var token = GetToken(login, password);

            CheckForExist(login);

            _securedDataProvider.Store(login, ConstantsService.ProviderName,
                new Dictionary<string, string> { { _clientAuthorization.GetNote(), token } });

            UserManager.SetLastUser(login);
            UserManager.AddedUsers.Add(login);
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
            var retreiveResponce = _securedDataProvider.Retreive(ConstantsService.ProviderName, login)?.Username;

            if ( retreiveResponce == null ) return;

            _securedDataProvider.Clear(retreiveResponce);

            UserManager.DeletedUsers.Add(login);
        }

    }
}
