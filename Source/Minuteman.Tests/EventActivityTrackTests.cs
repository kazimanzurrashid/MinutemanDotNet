namespace Minuteman.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public abstract class EventActivityTrackTests : IDisposable
    {
        protected const string EventName = "my-event";
        protected static readonly DateTime Timestamp =
            new DateTime(2013, 9, 1, 15, 28, 00);

        protected EventActivityTrackTests(ActivityDrilldown drilldown)
        {
            EventActivity = new EventActivity(
                new ActivitySettings(1, drilldown));
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

        protected async Task TestExists(ActivityDrilldown drilldown)
        {
            var key = EventActivity.GenerateKey(
                EventName,
                EventActivity.Settings.Drilldown.ToString());

            var field = EventActivity.GenerateTimeframeFields(
                drilldown,
                Timestamp)
                .ElementAt((int)drilldown);

            using (var connection = await ConnectionFactory.Open())
            {
                var result = await connection.Hashes
                    .Exists(EventActivity.Settings.Db, key, field);

                Assert.True(result);
            }
        }
    }
}