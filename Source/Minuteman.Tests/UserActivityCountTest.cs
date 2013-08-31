namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityCountTest : IDisposable
    {
        private const string EventName = "my-event";
        private readonly UserActivity userActivity;

        public UserActivityCountTest()
        {
            userActivity = new UserActivity(
                new ActivitySettings(1, ActivityDrilldown.Year));
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task ReturnsTrackedEventsCount()
        {
            var timestamp = DateTime.UtcNow;

            await userActivity.Track(EventName, timestamp, 100, 204, 1002, 3);

            var count = await userActivity.Users(EventName, timestamp).Count();

            Assert.Equal(4, count);
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }
    }
}