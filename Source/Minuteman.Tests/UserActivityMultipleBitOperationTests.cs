namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMultipleBitOperationTests : IDisposable
    {
        private readonly UserActivity userActivity;

        public UserActivityMultipleBitOperationTests()
        {
            userActivity = new UserActivity(
                new ActivitySettings(1, ActivityTimeframe.Year));

            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task AndOnlyIncludesCommon()
        {
            var timestamp = DateTime.UtcNow;

            await Task.WhenAll(
                userActivity.Track("bought-apple", timestamp, 1, 2, 3, 4),
                userActivity.Track("bought-banana", timestamp, 3, 4, 5, 6),
                userActivity.Track("bought-mango", timestamp, 2, 4, 6));

            var apple = userActivity.Report("bought-apple", timestamp);
            var banana = userActivity.Report("bought-banana", timestamp);
            var mango = userActivity.Report("bought-mango", timestamp);
            var appleAndBanana = apple & banana;

            var all = appleAndBanana & mango;
            var results = await all.Includes(1, 2, 3, 4, 5, 6);

            for (var i = 0; i < results.Length; i++)
            {
                if (i == 3)
                {
                    Assert.True(results[i]);
                }
                else
                {
                    Assert.False(results[i]);
                }
            }
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }
    }
}