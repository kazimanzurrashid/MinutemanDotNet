namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityDayTrackTests : EventActivityMonthTrackTests
    {
        public EventActivityDayTrackTests() :
            this(ActivityTimeframe.Day)
        {
        }

        protected EventActivityDayTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesDayField()
        {
            await TestExists(ActivityTimeframe.Day);
        }
    }
}