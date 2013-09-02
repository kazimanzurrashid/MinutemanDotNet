namespace Minuteman.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class UserActivityTrackTests : IDisposable
    {
        protected const string EventName = "my-event";
        protected static readonly DateTime Timestamp = DateTime.UtcNow;

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

        protected async Task TestExists(ActivityDrilldown drilldown)
        {
            var key = UserActivity.GenerateEventTimeframeKeys(
                EventName,
                drilldown,
                Timestamp)
                .ElementAt((int)drilldown);

            using (var connection = await ConnectionFactories.Open())
            {
                var result = await connection.Strings.Get(
                    UserActivity.Settings.Db,
                    key);

                Assert.NotNull(result);
            }
        }
    }
}