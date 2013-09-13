namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivityMinuteTrackTests : EventActivityHourTrackTests
    {
        public EventActivityMinuteTrackTests() :
            this(ActivityTimeframe.Minute)
        {
        }

        protected EventActivityMinuteTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesMinuteField()
        {
            await TestExists(ActivityTimeframe.Minute);
        }
    }
}