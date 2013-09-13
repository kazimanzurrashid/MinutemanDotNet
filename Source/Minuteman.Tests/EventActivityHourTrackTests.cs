namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityHourTrackTests : EventActivityDayTrackTests
    {
        public EventActivityHourTrackTests() :
            this(ActivityTimeframe.Hour)
        {
        }

        protected EventActivityHourTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesHourField()
        {
            await TestExists(ActivityTimeframe.Hour);
        }
    }
}