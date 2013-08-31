namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityXorCountTests : IDisposable
    {
        private readonly UserActivity userActivity;

        public UserActivityXorCountTests()
        {
            userActivity = new UserActivity(
                new ActivitySettings(1, ActivityDrilldownType.Year));

            userActivity.Reset().Wait();
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task ReturnsSumOfExclusives()
        {
            var timestamp = DateTime.UtcNow;

            await Task.WhenAll(
                userActivity.Track("bought-apple", timestamp, 1, 2, 3, 4),
                userActivity.Track("bought-banana", timestamp, 3, 4, 5, 6));

            var apple = userActivity.Users("bought-apple", timestamp);
            var banana = userActivity.Users("bought-banana", timestamp);
            var both = apple ^ banana;
            var count = await both.Count();

            Assert.Equal(4, count);
        }
    }
}