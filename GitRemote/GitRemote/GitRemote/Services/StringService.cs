using System.Linq;

namespace GitRemote.Services
{
    public class StringService
    {
        public static bool CheckForNullOrEmpty(params string[] strings)
        {
            return strings.All(current => !string.IsNullOrEmpty(current));
        }
    }
}
