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
    }
}
