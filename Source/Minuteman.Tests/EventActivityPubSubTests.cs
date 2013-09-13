namespace Minuteman.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityPubSubTests: IDisposable
    {
        private readonly EventActivity activity;

        public EventActivityPubSubTests()
        {
            activity = new EventActivity(
                new ActivitySettings(1, ActivityTimeframe.Year));
            activity.Reset().Wait();
        }

        [Fact]
        public async Task ExchangesMessage()
        {
            const string EventName = "order-placed";
            var timestamp = DateTime.UtcNow;

            var signal = new ManualResetEvent(false);

            var subscription = activity.CreateSubscription();

            await subscription.Subscribe(EventName, e =>
                {
                    Assert.Equal(EventName, e.EventName);
                    Assert.Equal(timestamp, e.Timestamp);
                    Assert.Equal(ActivityTimeframe.Year, e.Timeframe);
                    Assert.Equal(1, e.Count);
                    signal.Set();
                });

            await activity.Track(EventName, timestamp, true);

            signal.WaitOne(TimeSpan.FromSeconds(1));

            await subscription.Unsubscribe(EventName);
            subscription.Dispose();
        }

        public void Dispose()
        {
            activity.Reset().Wait();
        }
    }
}