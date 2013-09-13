namespace Minuteman.Tests
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityPubSubTests : IDisposable
    {
        private readonly UserActivity activity;

        public UserActivityPubSubTests()
        {
            activity = new UserActivity(
                new ActivitySettings(1, ActivityTimeframe.Year));
            activity.Reset().Wait();
        }

        public void Dispose()
        {
            activity.Reset().Wait();
        }

        [Fact]
        public async Task ExchangesMessage()
        {
            const string EventName = "login";
            var timestamp = DateTime.UtcNow;

            var signal = new ManualResetEvent(false);

            var subscription = activity.CreateSubscription();

            await subscription.Subscribe(
                EventName,
                e =>
                    {
                        Assert.Equal(EventName, e.EventName);
                        Assert.Equal(timestamp, e.Timestamp);
                        Assert.Contains(1, e.Users);
                        Assert.Contains(2, e.Users);
                        Assert.Contains(3, e.Users);
                        signal.Set();
                    });

            await activity.Track(EventName, true, 1, 2, 3);

            signal.WaitOne(TimeSpan.FromSeconds(1));

            await subscription.Unsubscribe(EventName);
            subscription.Dispose();
        }
    }
}