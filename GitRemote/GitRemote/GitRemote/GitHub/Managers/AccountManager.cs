using System.Collections.Generic;
using System.Threading.Tasks;
using GitRemote.DI;
using GitRemote.Services;
using Octokit;
using static GitRemote.Services.StringService.SoftStrings;

namespace GitRemote.GitHub.Managers
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
        /// <param name="token"></param>
        /// <param name="privateFeedUrl"></param>
        /// <returns>Token</returns>
        public void AddAccount(string login, string token, string privateFeedUrl)
        {
            CheckForExist(login);

            var securedDictionary = new Dictionary<string, string>
            {
                {_clientAuthorization.Note, token},
                {PrivateFeedUrl, privateFeedUrl}
            };

            _securedDataProvider.Store(login, ConstantsService.ProviderName, securedDictionary);
            UserManager.AddedUsers.Add(login);
        }

        /// <summary>
        /// Gets token if all is fine
        /// </summary>
        /// <returns>Token</returns>
        public async Task<string> GetTokenAsync(GitHubClient gitHubClient)
        {
            var token = await _clientAuthorization.GenerateTokenAsync(gitHubClient);

            return token;
        }

        /// <summary>
        /// Gets token for 2FA account
        /// </summary>
        /// <param name="gitHubClient">Client with his credentials</param>
        /// <param name="twoFactorAuthCode">Code from CodeEntry</param>
        /// <returns>Token</returns>
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

            if (retreiveResponce == null) return;

            _securedDataProvider.Clear(retreiveResponce);

            UserManager.DeletedUsers.Add(login);
        }

    }
}
