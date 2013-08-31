namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityDayTrackTests : EventActivityMonthTrackTests
    {
        public EventActivityDayTrackTests() :
            this(ActivityDrilldown.Day)
        {
        }

        protected EventActivityDayTrackTests(
            ActivityDrilldown drilldown)
            : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesDayField()
        {
            await TestExists(ActivityDrilldown.Day);
        }
    }
}