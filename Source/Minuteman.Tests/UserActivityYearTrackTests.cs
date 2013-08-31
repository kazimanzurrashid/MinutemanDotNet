namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityYearTrackTests : UserActivityTrackTests
    {
        public UserActivityYearTrackTests() :
            this(ActivityDrilldown.Year)
        {
        }

        protected UserActivityYearTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesYearEntry()
        {
            await TestExists(ActivityDrilldown.Year);
        }
    }
}