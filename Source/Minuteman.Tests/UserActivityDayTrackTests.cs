namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityDayTrackTests : UserActivityMonthTrackTests
    {
        public UserActivityDayTrackTests()
            : this(ActivityDrilldown.Day)
        {
        }

        protected UserActivityDayTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesDayEntry()
        {
            await TestExists();
        }
    }
}