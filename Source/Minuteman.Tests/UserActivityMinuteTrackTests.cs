namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMinuteTrackTests : UserActivityHourTrackTests
    {
        public UserActivityMinuteTrackTests()
            : this(ActivityDrilldown.Minute)
        {
        }

        protected UserActivityMinuteTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMinuteEntry()
        {
            await TestExists(ActivityDrilldown.Minute);
        }
    }
}