namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityDayTrackTests : UserActivityMonthTrackTests
    {
        public UserActivityDayTrackTests()
            : this(ActivityDrilldownType.Day)
        {
        }

        protected UserActivityDayTrackTests(
            ActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesDayEntry()
        {
            await TestExists();
        }
    }
}