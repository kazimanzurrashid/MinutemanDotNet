namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public static class UserActivityExtensions
    {
        public static Task Track(
            this IUserActivity instance,
            string eventName,
            params long[] users)
        {
            return Track(instance, eventName, false, users);
        }

        public static Task Track(
            this IUserActivity instance,
            string eventName,
            bool publishable,
            params long[] users)
        {
            return Track(
                instance,
                eventName,
                DateTime.UtcNow, 
                publishable,
                users);
        }

        public static Task Track(
            this IUserActivity instance,
            string eventName,
            DateTime timestamp,
            params long[] users)
        {
            return Track(instance, eventName, timestamp, false, users);
        }

        public static Task Track(
            this IUserActivity instance,
            string eventName,
            DateTime timestamp,
            bool publishable,
            params long[] users)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Track(
                eventName,
                instance.Settings.Timeframe,
                timestamp,
                publishable,
                users);
        }

        public static UserActivityReport Report(
            this IUserActivity instance,
            string eventName)
        {
            return Report(instance, eventName, DateTime.UtcNow);
        }

        public static UserActivityReport Report(
            this IUserActivity instance,
            string eventName,
            DateTime timestamp)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Report(
                eventName,
                instance.Settings.Timeframe,
                timestamp);
        }
    }
}