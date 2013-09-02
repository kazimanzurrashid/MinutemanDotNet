namespace Minuteman.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityPubSubTests: IDisposable
    {
        private readonly EventActivity publisher;

        public EventActivityPubSubTests()
        {
            publisher = new EventActivity(
                new ActivitySettings(1, ActivityDrilldown.Year));
            publisher.Reset().Wait();
        }

        [Fact]
        public async Task ExchangesMessage()
        {
            const string EventName = "order-placed";
            var timestamp = DateTime.UtcNow;

            var signal = new ManualResetEvent(false);

            var subscription = publisher.CreateSubscription(
                EventName,
                e =>
                    {
                        Assert.Equal(EventName, e.EventName);
                        Assert.Equal(timestamp, e.Timestamp);
                        Assert.Equal(ActivityDrilldown.Year, e.Drilldown);
                        Assert.Equal(1, e.Count);
                        signal.Set();
                    });

            await subscription.Subscribe();

            await publisher.Track(EventName, timestamp, true);

            signal.WaitOne(TimeSpan.FromSeconds(1));

            await subscription.Unsubscribe();
            subscription.Dispose();
        }

        public void Dispose()
        {
            publisher.Reset().Wait();
        }
    }
}