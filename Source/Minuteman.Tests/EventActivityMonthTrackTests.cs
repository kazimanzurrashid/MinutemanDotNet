namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityMonthTrackTests : EventActivityYearTrackTests
    {
        public EventActivityMonthTrackTests() :
            this(ActivityDrilldown.Month)
        {
        }

        protected EventActivityMonthTrackTests(
            ActivityDrilldown drilldown)
            : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMonthField()
        {
            await TestExists(ActivityDrilldown.Month);
        }
    }
}