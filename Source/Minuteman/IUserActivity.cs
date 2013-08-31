namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IUserActivity
    {
        ActivitySettings Settings { get; }

        Task Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp,
            params long[] users);

        UsersResult Users(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp);

        Task<IEnumerable<string>> EventNames();

        Task<long> Reset();
    }
}