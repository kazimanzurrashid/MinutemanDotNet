namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMinuteTrackTests : UserActivityHourTrackTests
    {
        public UserActivityMinuteTrackTests()
            : this(ActivityTimeframe.Minute)
        {
        }

        protected UserActivityMinuteTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesMinuteEntry()
        {
            await TestExists(ActivityTimeframe.Minute);
        }
    }
}