using System.Linq;

namespace GitRemote.Services
{
    public class StringService
    {
        /// <summary>
        /// Checks if all of strings not null or empty
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static bool CheckForNullOrEmpty(params string[] strings)
        {
            return strings.All(s => !string.IsNullOrEmpty(s));
        }

        public static bool IsNullOrEmpty(params string[] strings)
        {
            var res = strings.Any(string.IsNullOrEmpty);
            return res;
        }

        public static class SoftStrings
        {
            public const string PrivateFeedUrl = nameof(PrivateFeedUrl);
            public const string Space = " ";
            public const string Empty = "";
            public const string OwnerName = nameof(OwnerName);
            public const string ReposName = nameof(ReposName);
        }
    }
}
