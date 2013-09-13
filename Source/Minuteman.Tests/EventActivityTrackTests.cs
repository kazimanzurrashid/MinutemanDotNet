namespace Minuteman.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class EventActivityTrackTests : IDisposable
    {
        protected const string EventName = "my-event";
        protected static readonly DateTime Timestamp = DateTime.UtcNow;

        protected EventActivityTrackTests(ActivityTimeframe timeframe)
        {
            EventActivity = new EventActivity(
                new ActivitySettings(1, timeframe));
            EventActivity.Reset().Wait();
            EventActivity.Track(EventName, Timestamp).Wait();
        }

        protected EventActivity EventActivity { get; private set; }

        [Fact]
        public async Task AddsEventNameInTheEventsMember()
        {
            var events = await EventActivity.EventNames();

            Assert.Contains(EventName, events);
        }

        public void Dispose()
        {
            EventActivity.Reset().Wait();
        }

        protected async Task TestExists(ActivityTimeframe timeframe)
        {
            var key = EventActivity.GenerateKey(
                EventName,
                EventActivity.Settings.Timeframe.ToString());

            var field = EventActivity.GenerateTimeframeFields(
                timeframe,
                Timestamp)
                .ElementAt((int)timeframe);

            using (var connection = await ConnectionFactories.Open())
            {
                var result = await connection.Hashes
                    .Exists(EventActivity.Settings.Db, key, field);

                Assert.True(result);
            }
        }
    }
}