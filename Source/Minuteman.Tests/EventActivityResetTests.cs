namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityResetTests
    {
        private readonly EventActivity eventActivity;

        public EventActivityResetTests()
        {
            eventActivity = new EventActivity(
                new ActivitySettings(1, ActivityTimeframe.Year));
            eventActivity.Reset().Wait();
        }

        [Fact]
        public async Task RemovesAllTrackedEvents()
        {
            await Task.WhenAll(
                eventActivity.Track("foo"),
                eventActivity.Track("bar"),
                eventActivity.Track("baz"));

            var count = await eventActivity.Reset();

            // 3 tracked events, 3 for the drilldowns and 1 for event names
            Assert.Equal(7, count);
        }
    }
}