namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityYearTrackTests : EventActivityTrackTests
    {
        public EventActivityYearTrackTests() : this(ActivityTimeframe.Year)
        {
        }

        protected EventActivityYearTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesYearField()
        {
            await TestExists(ActivityTimeframe.Year);
        }
    }
}