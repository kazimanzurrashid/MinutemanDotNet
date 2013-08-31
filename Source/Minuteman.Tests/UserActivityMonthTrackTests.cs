namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMonthTrackTests : UserActivityYearTrackTests
    {
        public UserActivityMonthTrackTests() :
            this(ActivityDrilldown.Month)
        {
        }

        protected UserActivityMonthTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMonthEntry()
        {
            await TestExists(ActivityDrilldown.Month);
        }
    }
}