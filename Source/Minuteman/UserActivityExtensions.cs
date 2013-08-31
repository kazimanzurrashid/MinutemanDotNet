namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public static class UserActivityExtensions
    {
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

        public static UsersResult Users(
            this IUserActivity instance,
            string eventName,
            DateTime timestamp)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return instance.Users(
                eventName,
                instance.Settings.Drilldown,
                timestamp);
        }
    }
}