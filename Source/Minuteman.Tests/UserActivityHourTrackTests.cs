namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityHourTrackTests : UserActivityDayTrackTests
    {
        public UserActivityHourTrackTests()
            : this(ActivityDrilldownType.Hour)
        {
        }

        protected UserActivityHourTrackTests(
            ActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesHourEntry()
        {
            await TestExists();
        }
    }
}