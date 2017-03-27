using System;

namespace GitRemote.Services
{
    public static class ConstantsService
    {
        public static string ProviderName = "GitRemoteAuth"; // Unique name for purpose to find accounts for appication

        /// <summary>
        /// ProductHeader
        /// </summary>
        public static string AppName = "GitRemote";
        public static readonly DateTime Jan1St1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly string TwoFactorAuthUrl = "https://help.github.com/articles/about-two-factor-authentication/";
        public static readonly string GitHubOfficialPageUrl = "https://github.com/";
        public const string AtomNamespace = "http://www.w3.org/2005/Atom";
        public const int MaxNormalWidthForTitle = 450;
        public const int OtherWidth = 95;
        public const string GitHubApiLink = "https://api.github.com";

    }
}
