namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityDayTrackTests : UserActivityMonthTrackTests
    {
        public UserActivityDayTrackTests()
            : this(ActivityTimeframe.Day)
        {
        }

        protected UserActivityDayTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesDayEntry()
        {
            await TestExists(ActivityTimeframe.Day);
        }
    }
}