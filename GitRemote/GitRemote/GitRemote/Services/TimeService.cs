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

        /// <summary>
        /// Converts a DateTime value to a friendly string like "8 hours ago" using UTC time
        /// </summary>
        public static string ConvertToFriendly(string value)
        {
            if ( value == null ) return string.Empty;

            var totalTime = string.Empty;

            var ts = DateTime.Now - Convert.ToDateTime(value);

            if ( ts.Days > 7 )
                totalTime = Convert.ToDateTime(value).Date.ToString("d");
            else if ( ts.Days > 0 )
            {
                totalTime = totalTime + ts.Days + " day";
                if ( ts.Days > 1 ) totalTime = totalTime + 's';
                totalTime = totalTime + " ago";
            }
            else if ( ts.Hours > 0 )
            {
                totalTime = totalTime + ts.Hours + " hour";
                if ( ts.Hours > 1 ) totalTime = totalTime + 's';
                totalTime = totalTime + " ago";
            }
            else if ( ts.Minutes > 0 )
            {
                totalTime = totalTime + ts.Minutes + " minute";
                if ( ts.Minutes > 1 ) totalTime = totalTime + 's';
                totalTime = totalTime + " ago";
            }
            return totalTime;
        }
    }
}
