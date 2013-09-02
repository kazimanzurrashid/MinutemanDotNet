namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public static class EventActivityExtensions
    {
        public static Task Track(
            this IEventActivity instance,
            string eventName)
        {
            return Track(instance, eventName, DateTime.UtcNow);
        }

        public static Task Track(
            this IEventActivity instance,
            string eventName,
            bool publishable)
        {
            return Track(instance, eventName, DateTime.UtcNow, publishable);
        }

        public static Task Track(
            this IEventActivity instance,
            string eventName,
            DateTime timestamp)
        {
            return Track(instance, eventName, timestamp, false);
        }

        public static Task Track(
            this IEventActivity instance,
            string eventName,
            DateTime timestamp,
            bool publishable)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Track(
                eventName,
                instance.Settings.Drilldown,
                timestamp,
                publishable);
        }

        public static Task<long[]> Counts(
            this IEventActivity instance,
            string eventName,
            DateTime startTimestamp,
            DateTime endTimestamp)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Counts(
                eventName,
                startTimestamp,
                endTimestamp,
                instance.Settings.Drilldown);
        }
    }
}