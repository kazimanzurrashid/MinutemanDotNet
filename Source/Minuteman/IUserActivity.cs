namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserActivity
    {
        UserActivitySettings Settings { get; }

        Task Track(
            string eventName,
            UserActivityDrilldownType drilldown,
            DateTime timestamp,
            params long[] users);

        UsersResult Users(
            string eventName,
            UserActivityDrilldownType drilldown,
            DateTime timestamp);

        Task<IEnumerable<string>> EventNames();

        Task<long> Reset();
    }
}