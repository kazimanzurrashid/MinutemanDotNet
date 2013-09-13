namespace Minuteman.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    [CLSCompliant(false)]
    public class UserActivityHub : ActivityHub
    {
        private static readonly IUserActivity Activity = new UserActivity();
        private static readonly ISubscription<UserActivitySubscriptionInfo> Subscription = Activity.CreateSubscription();
        private static readonly Func<Task<IEnumerable<string>>> GetEventNames = () => Activity.EventNames(true);
        private static readonly Func<string, Task<IEnumerable<ActivityTimeframe>>> GetTimeframes = eventName => Activity.Timeframes(eventName);

        private readonly ISubscription<UserActivitySubscriptionInfo> subscription;

        public UserActivityHub()
            : this(Subscription, GetEventNames, GetTimeframes)
        {
        }

        public UserActivityHub(
            ISubscription<UserActivitySubscriptionInfo> subscription,
            Func<Task<IEnumerable<string>>> getEventNames,
            Func<string, Task<IEnumerable<ActivityTimeframe>>> getTimeframes)
            : base(getEventNames, getTimeframes)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException("subscription");
            }

            this.subscription = subscription;
        }

        protected override Task OnSubscribe(string eventName)
        {
            return subscription.Subscribe(eventName, OnDataReceive);
        }

        protected override Task OnUnsubscribe(string eventName)
        {
            return subscription.Unsubscribe(eventName);
        }

        private void OnDataReceive(UserActivitySubscriptionInfo info)
        {
            Clients.All.update(
                info.EventName,
                info.Timeframe.ToString(),
                info.Timestamp,
                info.Users);
        }
    }
}