namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IEventActivity : IActivity<EventActivitySubscriptionInfo>
    {
        Task<long> Track(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp,
            bool publishable);

        Task<long[]> Counts(
            string eventName,
            DateTime startTimestamp,
            DateTime endTimestamp,
            ActivityTimeframe timeframe);
    }
}