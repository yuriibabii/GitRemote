using GitRemote.DI;
using GitRemote.Services;
using Octokit;
using Octokit.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitRemote.GitHub
{
    public class AccountManager
    {
        private readonly ClientAuthorization _clientAuthorization;
        private readonly ISecuredDataProvider _securedDataProvider;

        public AccountManager(ClientAuthorization clientAuthorization,
                              ISecuredDataProvider securedDataProvider)
        {
            _clientAuthorization = clientAuthorization;
            _securedDataProvider = securedDataProvider;
        }

        /// <summary>
        /// Takes token, checks for exist of account with same login, saves account and returns token for this account
        /// </summary>
        /// <param name="login"></param>
        /// <param name="token"></param>
        /// <returns>Token</returns>

        public void AddAccount(string login, string token)
        {
            CheckForExist(login);

            _securedDataProvider.Store(login, ConstantsService.ProviderName,
                new Dictionary<string, string> { { _clientAuthorization.Note, token } });

            UserManager.AddedUsers.Add(login);
        }

        /// <summary>
        /// Gets token if all is fine
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns>Token</returns>
        public async Task<string> GetTokenAsync(string login, string password)
        {
            var gitHubClient = new GitHubClient(new ProductHeaderValue(ConstantsService.AppName),
                                   new InMemoryCredentialStore(new Credentials(login, password)));
            var token = await _clientAuthorization.GenerateTokenAsync(gitHubClient);

            return token;
        }

        public async Task<string> GetTokenAsync(GitHubClient gitHubClient, string twoFactorAuthCode)
        {
            var token = await _clientAuthorization.GenerateTokenWithCodeAsync(gitHubClient, twoFactorAuthCode);
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
