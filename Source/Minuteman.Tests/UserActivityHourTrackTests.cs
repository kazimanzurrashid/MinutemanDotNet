namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityHourTrackTests : UserActivityDayTrackTests
    {
        public UserActivityHourTrackTests()
            : this(UserActivityDrilldownType.Hour)
        {
        }

        protected UserActivityHourTrackTests(
            UserActivityDrilldownType drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesHourEntry()
        {
            await TestExists();
        }
    }
}