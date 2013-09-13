namespace Minuteman.Dashboard
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    [CLSCompliant(false)]
    public class EventActivityHub : ActivityHub
    {
        private static readonly IEventActivity Activity = new EventActivity();
        private static readonly ISubscription<EventActivitySubscriptionInfo> Subscription = Activity.CreateSubscription();
        private static readonly Func<Task<IEnumerable<string>>> GetEventNames = () => Activity.EventNames(true); 
        private static readonly Func<string, Task<IEnumerable<ActivityTimeframe>>> GetTimeframes = eventName => Activity.Timeframes(eventName);

        private readonly ISubscription<EventActivitySubscriptionInfo> subscription;

        public EventActivityHub() : this(Subscription, GetEventNames, GetTimeframes)
        {
        }

        public EventActivityHub(
            ISubscription<EventActivitySubscriptionInfo> subscription,
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

        private void OnDataReceive(EventActivitySubscriptionInfo info)
        {
            Clients.All.update(JsonConvert.SerializeObject(info, SerializerSettings));
        }
    }
}