namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityOrIncludesTests : IDisposable
    {
        private readonly UserActivity userActivity;

        public UserActivityOrIncludesTests()
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
        public async Task IncludesOnlyUniques()
        {
            var timestamp = DateTime.UtcNow;

            await Task.WhenAll(
                userActivity.Track("bought-apple", timestamp, 1, 2, 3, 4),
                userActivity.Track("bought-banana", timestamp, 3, 4, 5, 6));

            var apple = userActivity.Report("bought-apple", timestamp);
            var banana = userActivity.Report("bought-banana", timestamp);
            var both = apple | banana;

            var results = await both.Includes(0, 1, 2, 3, 4, 5, 6, 7);

            for (var i = 0; i < results.Length; i++)
            {
                if (i > 0 && i < 7)
                {
                    Assert.True(results[i]);
                }
                else
                {
                    Assert.False(results[i]);
                }
            }
        }
    }
}