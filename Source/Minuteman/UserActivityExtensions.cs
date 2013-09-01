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
            return Track(instance, eventName, DateTime.UtcNow, users);
        }

        public static Task Track(
            this IUserActivity instance,
            string eventName,
            DateTime timestamp,
            params long[] users)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Track(
                eventName,
                instance.Settings.Drilldown,
                timestamp,
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
                instance.Settings.Drilldown,
                timestamp);
        }
    }
}