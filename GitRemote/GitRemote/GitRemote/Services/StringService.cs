using System.Linq;

namespace GitRemote.Services
{
    public class StringService
    {
        public static bool CheckForNullOrEmpty(params string[] strings)
        {
            return strings.All(s => !string.IsNullOrEmpty(s));
        }
    }
}
