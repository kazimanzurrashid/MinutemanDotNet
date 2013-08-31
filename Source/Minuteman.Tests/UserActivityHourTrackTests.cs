namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityHourTrackTests : UserActivityDayTrackTests
    {
        public UserActivityHourTrackTests()
            : this(ActivityDrilldown.Hour)
        {
        }

        protected UserActivityHourTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesHourEntry()
        {
            await TestExists(ActivityDrilldown.Hour);
        }
    }
}