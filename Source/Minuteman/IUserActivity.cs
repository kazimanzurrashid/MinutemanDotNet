namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IUserActivity : IActivity
    {
        Task Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp,
            params long[] users);

        UserActivityReport Report(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp);
    }
}