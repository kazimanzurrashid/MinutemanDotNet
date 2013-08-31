namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class UserActivityTrackTests : IDisposable
    {
        protected const string EventName = "my-event";
        protected static readonly DateTime Timestamp =
            new DateTime(2013, 8, 30, 10, 26, 00);

        protected UserActivityTrackTests(ActivityDrilldown drilldown)
        {
            UserActivity = new UserActivity(
                new ActivitySettings(1, drilldown));
            UserActivity.Reset().Wait();
            UserActivity.Track(EventName, Timestamp, 1, 2, 3).Wait();
        }

        protected UserActivity UserActivity { get; private set; }

        [Fact]
        public async Task AddsEventNameInTheEventsMember()
        {
            var events = await UserActivity.EventNames();

            Assert.Contains(EventName, events);
        }

        public void Dispose()
        {
            UserActivity.Reset().Wait();
        }
    }
}