namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityMonthTrackTests : EventActivityYearTrackTests
    {
        public EventActivityMonthTrackTests() :
            this(ActivityTimeframe.Month)
        {
        }

        protected EventActivityMonthTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesMonthField()
        {
            await TestExists(ActivityTimeframe.Month);
        }
    }
}