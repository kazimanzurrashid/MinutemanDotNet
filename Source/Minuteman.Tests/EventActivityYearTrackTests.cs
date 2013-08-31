namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityYearTrackTests : EventActivityTrackTests
    {
        public EventActivityYearTrackTests() : this(ActivityDrilldown.Year)
        {
        }

        protected EventActivityYearTrackTests(
            ActivityDrilldown drilldown)
            : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesYearField()
        {
            await TestExists(ActivityDrilldown.Year);
        }
    }
}