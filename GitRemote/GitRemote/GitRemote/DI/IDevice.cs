using System;
using System.Threading.Tasks;

namespace GitRemote.DI
{
    public interface IDevice
    {
        /// <summary>
        /// Starts the default app associated with the URI for the specified URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>The launch operation.</returns>
        Task<bool> LaunchUriAsync(Uri uri);
    }
}
