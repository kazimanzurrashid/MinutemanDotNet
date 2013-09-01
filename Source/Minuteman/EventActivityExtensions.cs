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
            DateTime timestamp)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Track(
                eventName, 
                instance.Settings.Drilldown,
                timestamp);
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