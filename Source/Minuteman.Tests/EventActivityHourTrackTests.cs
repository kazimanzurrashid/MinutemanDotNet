namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityHourTrackTests : EventActivityDayTrackTests
    {
        public EventActivityHourTrackTests() :
            this(ActivityDrilldown.Hour)
        {
        }

        protected EventActivityHourTrackTests(
            ActivityDrilldown drilldown)
            : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesHourField()
        {
            await TestExists(ActivityDrilldown.Hour);
        }
    }
}