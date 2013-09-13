namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IUserActivity : IActivity<UserActivitySubscriptionInfo>
    {
        Task Track(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp,
            bool publishable,
            params long[] users);

        UserActivityReport Report(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp);
    }
}