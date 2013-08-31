namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityMinuteTrackTests : EventActivityHourTrackTests
    {
        public EventActivityMinuteTrackTests() :
            this(ActivityDrilldown.Minute)
        {
        }

        protected EventActivityMinuteTrackTests(
            ActivityDrilldown drilldown)
            : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesMinuteField()
        {
            await TestExists(ActivityDrilldown.Minute);
        }
    }
}