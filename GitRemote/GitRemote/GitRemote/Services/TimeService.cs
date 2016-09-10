using System;

namespace GitRemote.Services
{
    public static class TimeService
    {
        public static long CurrentTimeMillis()
        {
            return ( long )( DateTime.UtcNow - ConstantsService.Jan1St1970 ).TotalMilliseconds;
        }
    }
}
