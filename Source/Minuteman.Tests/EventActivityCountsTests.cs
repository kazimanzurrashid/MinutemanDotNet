namespace Minuteman.Tests
{
    using System;
    using System.Linq;

    using Xunit;

    public class EventActivityCountsTests : IDisposable
    {
        private readonly EventActivity eventActivity;

        public EventActivityCountsTests()
        {
            eventActivity = new EventActivity(new ActivitySettings(1));
            eventActivity.Reset().Wait();
        }

        [Fact]
        public async void ReturnsCounts()
        {
            const string EventName = "my-event";
            var now = DateTime.UtcNow;

            await eventActivity.Track(EventName, now);
            await eventActivity.Track(EventName, now);

            var counts = await eventActivity.Counts(EventName, now, now);

            Assert.Equal(2, counts.First());
        }

        [Fact]
        public async void ReturnsZeroWhenNotTracked()
        {
            const string EventName = "my-events-2";
            var now = DateTime.UtcNow;

            await eventActivity.Track(EventName, now);
            await eventActivity.Track(EventName, now.AddHours(2));

            var counts = await eventActivity.Counts(
                EventName,
                now,
                now.AddHours(2));

            Assert.Equal(1, counts.First());
            Assert.Equal(0, counts.ElementAt(1));
            Assert.Equal(1, counts.Last());
        }

        public void Dispose()
        {
            eventActivity.Reset().Wait();
        }
    }
}