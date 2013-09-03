namespace Minuteman.TestApp
{
    using System.Threading.Tasks;

    using Microsoft.AspNet.SignalR;

    public class LiveFeedsHub : Hub
    {
        private readonly ISubscription subscription;

        public LiveFeedsHub()
        {
            var eventActivity = new EventActivity(
                new ActivitySettings(ActivityDrilldown.Second));

            subscription = eventActivity.CreateSubscription(
                "order:placed",
                OnDataReceive);
        }

        public override Task OnConnected()
        {
            return subscription.Subscribe();
        }

        public override Task OnDisconnected()
        {
            return subscription.Unsubscribe();
        }

        private void OnDataReceive(EventActivitySubscriptionInfo info)
        {
            Clients.All.update(
                info.EventName,
                info.Drilldown.ToString(),
                info.Timestamp,
                info.Count);
        }
    }
}