namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMonthTrackTests : UserActivityYearTrackTests
    {
        public UserActivityMonthTrackTests() :
            this(ActivityDrilldownType.Month)
        {
        }

        protected UserActivityMonthTrackTests(
            ActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMonthEntry()
        {
            await TestExists();
        }
    }
}