namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityHourTrackTests : UserActivityDayTrackTests
    {
        public UserActivityHourTrackTests()
            : this(ActivityTimeframe.Hour)
        {
        }

        protected UserActivityHourTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesHourEntry()
        {
            await TestExists(ActivityTimeframe.Hour);
        }
    }
}