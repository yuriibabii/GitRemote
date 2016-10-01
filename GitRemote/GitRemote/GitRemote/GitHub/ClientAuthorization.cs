using GitRemote.Services;
using Octokit;
using Prism.Navigation;
using System;
using System.Threading.Tasks;
using TwoFactorAuthPage = GitRemote.Views.Authentication.TwoFactorAuthPage;

namespace GitRemote.GitHub
{
    public class ClientAuthorization
    {
        private readonly INavigationService _navigationService;
        public string Note { get; set; }
        public ClientAuthorization(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public ClientAuthorization()
        { }
        /// <summary>
        /// Checks accoung for 2FA and if it is, then we notify about it our methods and goes to 2FA page.
        /// If it is not, then we get token from Github API and generates unique note.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Token</returns>
        public async Task<string> GenerateTokenAsync(GitHubClient client)
        {
            var newAuthorization = GetNewAuthorization("user", "repo", "gist");
            try
            {
                var getTokenTask = await client.Authorization.Create(newAuthorization);
                return getTokenTask.Token;
            }
            catch ( TwoFactorRequiredException )
            {
                var parameters = new NavigationParameters { { "Client", client } };
                await _navigationService.NavigateAsync($"{nameof(TwoFactorAuthPage)}", parameters, animated: false);
                return "2FA";
            }
            catch ( Exception ex )
            {
                throw new Exception(ex.Message + " (Something wrong with token generation)");
            }
        }

        /// <summary>
        /// Generates token for 2FA account
        /// </summary>
        /// <param name="client"></param>
        /// <param name="twoFactorAuthCode"></param>
        /// <returns></returns>
        public async Task<string> GenerateTokenWithCodeAsync(GitHubClient client, string twoFactorAuthCode)
        {
            var newAuthorization = GetNewAuthorization("user", "repo", "gist");
            try
            {
                var getTokenTask = await client.Authorization.Create(newAuthorization, twoFactorAuthCode);
                return getTokenTask.Token;
            }
            catch ( Exception ex )
            {
                throw new Exception(ex.Message + " (Something wrong with 2FA token generation)");
            }
        }


        private NewAuthorization GetNewAuthorization(params string[] scopes)
        {
            var generatedTokenTime = TimeService.CurrentTimeMillis().ToString();
            Note = ConstantsService.AppName + ' ' + generatedTokenTime;
            var newAuthorization = new NewAuthorization(Note, scopes);
            return newAuthorization;
        }
    }
}
