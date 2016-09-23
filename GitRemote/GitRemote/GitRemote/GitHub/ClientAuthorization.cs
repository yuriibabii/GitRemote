using GitRemote.Services;
using Octokit;
using System;
using System.Collections.Generic;

namespace GitRemote.GitHub
{
    public class ClientAuthorization
    {
        private string _generatedTokenTime = string.Empty;
        private string _note = string.Empty;

        /// <summary>
        /// Gets token from Github API and generates unique note
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Token</returns>
        public string GenerateToken(GitHubClient client)
        {
            _generatedTokenTime = TimeService.CurrentTimeMillis().ToString();
            _note = ConstantsService.AppName + ' ' + _generatedTokenTime;
            try
            {
                return client.Authorization.Create(new NewAuthorization(_note, new List<string> { "user", "repo", "gist" })).Result.Token;
            }
            catch ( Exception )
            {
                return null;
            }
        }

        public string GetNote()
        {
            return _note;
        }
    }
}
