namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMonthTrackTests : UserActivityYearTrackTests
    {
        public UserActivityMonthTrackTests() :
            this(ActivityTimeframe.Month)
        {
        }

        protected UserActivityMonthTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesMonthEntry()
        {
            await TestExists(ActivityTimeframe.Month);
        }
    }
}