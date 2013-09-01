namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityResetTests
    {
        private readonly UserActivity userActivity;

        public UserActivityResetTests()
        {
            userActivity = new UserActivity(
                new ActivitySettings(1, ActivityDrilldown.Year));
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task RemovesAllTrackedEvents()
        {
            await Task.WhenAll(
                userActivity.Track("foo", 1),
                userActivity.Track("bar", 2),
                userActivity.Track("baz", 3));

            var count = await userActivity.Reset();

            // 3 tracked events and 1 for event names
            Assert.Equal(4, count);
        }
    }
}