namespace Minuteman.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityIncludesTest : IDisposable
    {
        private const string EventName = "my-event";

        private readonly UserActivity userActivity;

        public UserActivityIncludesTest()
        {
            userActivity = new UserActivity(
                new ActivitySettings(1, ActivityDrilldown.Year));
            userActivity.Reset().Wait();
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task RetunsTrueIfPreviouslyStored()
        {
            var timestamp = DateTime.UtcNow;

            await userActivity.Track(EventName, timestamp, 97);
            await userActivity.Track(EventName, timestamp, 98);
            await userActivity.Track(EventName, timestamp, 99);

            var result = await userActivity.Users(EventName, timestamp)
                .Includes(98);

            Assert.True(result.First());
        }

        [Fact]
        public async Task ReturnFalseIfPreviouslyNotStored()
        {
            var timestamp = DateTime.UtcNow;

            await userActivity.Track(EventName, timestamp, 101);
            await userActivity.Track(EventName, timestamp, 103);

            var result = await userActivity.Users(EventName, timestamp)
                .Includes(102);

            Assert.False(result.First());
        }
    }
}