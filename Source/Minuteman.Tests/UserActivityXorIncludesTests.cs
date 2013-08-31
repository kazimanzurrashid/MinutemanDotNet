namespace Minuteman.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityXorIncludesTests : IDisposable
    {
        private readonly UserActivity userActivity;

        public UserActivityXorIncludesTests()
        {
            userActivity = new UserActivity(
                new UserActivitySettings(1, UserActivityDrilldownType.Year));

            userActivity.Reset().Wait();
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task IncludesOnlyExclusives()
        {
            var timestamp = DateTime.UtcNow;

            await Task.WhenAll(
                userActivity.Track("bought-apple", timestamp, 1, 2, 3, 4),
                userActivity.Track("bought-banana", timestamp, 3, 4, 5, 6));

            var apple = userActivity.Users("bought-apple", timestamp);
            var banana = userActivity.Users("bought-banana", timestamp);
            var both = apple ^ banana;

            var results = await Task.WhenAll(
                both.Includes(0),
                both.Includes(1),
                both.Includes(2),
                both.Includes(3),
                both.Includes(4),
                both.Includes(5),
                both.Includes(6),
                both.Includes(7));

            for (var i = 0; i < results.ToList().Count; i++)
            {
                if ((i == 0) || (i > 2 && i < 5) || (i == 7))
                {
                    Assert.False(results[i]);
                }
                else
                {
                    Assert.True(results[i]);
                }
            }
        }
    }
}