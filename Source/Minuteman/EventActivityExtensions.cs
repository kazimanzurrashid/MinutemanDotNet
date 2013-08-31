namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public static class EventActivityExtensions
    {
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
    }
}