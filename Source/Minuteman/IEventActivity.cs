namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IEventActivity : IActivity
    {
        Task<long> Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp,
            bool publishable);

        Task<long[]> Counts(
            string eventName,
            DateTime startTimestamp,
            DateTime endTimestamp,
            ActivityDrilldown drilldown);

        ISubscription CreateSubscription(
            string eventName,
            Action<EventActivitySubscriptionInfo> action);
    }
}