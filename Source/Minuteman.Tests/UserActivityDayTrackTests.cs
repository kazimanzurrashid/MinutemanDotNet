namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityDayTrackTests : UserActivityMonthTrackTests
    {
        public UserActivityDayTrackTests()
            : this(UserActivityDrilldownType.Day)
        {
        }

        protected UserActivityDayTrackTests(
            UserActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesDayEntry()
        {
            await TestExists();
        }
    }
}