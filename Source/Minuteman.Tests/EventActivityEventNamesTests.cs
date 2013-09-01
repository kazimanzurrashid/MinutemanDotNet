namespace Minuteman.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityEventNamesTests : IDisposable
    {
        private readonly EventActivity eventActivity;

        public EventActivityEventNamesTests()
        {
            eventActivity = new EventActivity(new ActivitySettings(1));
            eventActivity.Reset().Wait();
        }

        public void Dispose()
        {
            eventActivity.Reset().Wait();
        }

        [Fact]
        public async Task ReturnsAllTrackedEventNames()
        {
            var inputedEvents = new List<string>();

            for (var i = 1; i <= 5; i++)
            {
                await eventActivity.Track("event-" + i);
            }

            var outputedEvents = await eventActivity.EventNames();

            inputedEvents.ForEach(n => Assert.Contains(n, outputedEvents));
        }
    }
}