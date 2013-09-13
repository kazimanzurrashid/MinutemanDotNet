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

        protected UserActivityTrackTests(ActivityTimeframe timeframe)
        {
            UserActivity = new UserActivity(
                new ActivitySettings(1, timeframe));
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

        protected async Task TestExists(ActivityTimeframe timeframe)
        {
            var key = UserActivity.GenerateEventTimeframeKeys(
                EventName,
                timeframe,
                Timestamp)
                .ElementAt((int)timeframe);

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