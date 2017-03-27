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
            var isPositive = true;

            var convertedTime = Convert.ToDateTime(value);
            var ts = DateTime.Now - convertedTime;
            if ( ts.TotalMinutes < 0 )
            {
                ts = convertedTime - DateTime.Now;
                isPositive = false;
            }

            if ( ts.Days > 7 )
                return Convert.ToDateTime(value).Date.ToString("d");

            if ( ts.Days > 0 )
            {
                totalTime = totalTime + ts.Days + " day";
                if ( ts.Days > 1 ) totalTime = totalTime + 's';
            }
            else if ( ts.Hours > 0 )
            {
                totalTime = totalTime + ts.Hours + " hour";
                if ( ts.Hours > 1 ) totalTime = totalTime + 's';
            }
            else if ( ts.Minutes > 0 )
            {
                totalTime = totalTime + ts.Minutes + " minute";
                if ( ts.Minutes > 1 ) totalTime = totalTime + 's';
            }

            if ( isPositive )
                totalTime = totalTime + " ago";
            else
                totalTime = "in " + totalTime;

            return totalTime;
        }
    }
}
