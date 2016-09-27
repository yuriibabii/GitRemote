using GitRemote.Services;
using GitRemote.Views;
using Octokit;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitRemote.GitHub
{
    public class ClientAuthorization
    {
        private readonly INavigationService _navigationService;

        private string _generatedTokenTime = string.Empty;
        private string _note = string.Empty;

        public ClientAuthorization(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        /// <summary>
        /// Checks accoung for 2FA and if it is, then we notify about it our methods and goes to 2FA page.
        /// If it is not, then we get token from Github API and generates unique note.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Token</returns>
        public async Task<string> GenerateTokenAsync(GitHubClient client)
        {
            _generatedTokenTime = TimeService.CurrentTimeMillis().ToString();
            _note = ConstantsService.AppName + ' ' + _generatedTokenTime;

            var newAuthorization = new NewAuthorization(_note, new List<string> { "user", "repo", "gist" });
            try
            {
                var getTokenTask = await client.Authorization.Create(newAuthorization);
                return getTokenTask.Token;
            }
            catch ( TwoFactorRequiredException )
            {
                await _navigationService.NavigateAsync($"{nameof(TwoFactorAuthPage)}", animated: false);
                return "2FA";
            }
            catch ( Exception ex )
            {
                throw new Exception(ex.Message + " (Something wrong with token generation)");
            }
        }

        public string GetNote()
        {
            return _note;
        }
    }
}
