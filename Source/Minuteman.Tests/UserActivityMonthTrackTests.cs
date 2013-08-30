namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMonthTrackTests : UserActivityYearTrackTests
    {
        public UserActivityMonthTrackTests() :
            this(UserActivityDrilldownType.Month)
        {
        }

        protected UserActivityMonthTrackTests(
            UserActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMonthEntry()
        {
            await TestExists();
        }
    }
}