using GitRemote.DI;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
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

        /// <summary>
        /// Takes token, checks for exist of account with same login, saves account and returns token for this account
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>Token</returns>
        public string AddAccountAndReturnToken(string login, string password)
        {
            var token = GetToken(login, password);

            CheckForExist(login);

            _securedDataProvider.Store(login, ConstantsService.ProviderName,
                new Dictionary<string, string> { { _clientAuthorization.GetNote(), token } });

            UserManager.AddedUsers.Add(login);

            return token;
        }

        /// <summary>
        /// Gets token if all is fine
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>Token</returns>
        private string GetToken(string login, string password)
        {
            var token = _clientAuthorization.GenerateToken(new GitHubClient
                (new ProductHeaderValue(ConstantsService.AppName),
                new InMemoryCredentialStore(new Credentials(login, password))));

            if ( token == null )
                throw new Exception("Something wrong with generating token");

            return token;
        }

        /// <summary>
        /// Checks for already exist account in storage
        /// </summary>
        /// <param name="login"></param>
        private void CheckForExist(string login)
        {
            var retreiveResponce = _securedDataProvider.Retreive(ConstantsService.ProviderName, login)?.Username;

            if ( retreiveResponce == null ) return;

            _securedDataProvider.Clear(retreiveResponce);

            UserManager.DeletedUsers.Add(login);
        }

    }
}
