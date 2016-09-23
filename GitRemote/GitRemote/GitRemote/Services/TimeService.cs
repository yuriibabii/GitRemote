using System;

namespace GitRemote.Services
{
    public static class TimeService
    {
        /// <summary>
        /// Takes difference between Jan1St1970 time and current time in milliseconds
        /// </summary>
        /// <returns></returns>
        public static long CurrentTimeMillis()
        {
            return ( long )( DateTime.UtcNow - ConstantsService.Jan1St1970 ).TotalMilliseconds;
        }
    }
}
