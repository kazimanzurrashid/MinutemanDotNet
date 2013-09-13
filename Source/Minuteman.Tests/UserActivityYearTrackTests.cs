namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityYearTrackTests : UserActivityTrackTests
    {
        public UserActivityYearTrackTests() :
            this(ActivityTimeframe.Year)
        {
        }

        protected UserActivityYearTrackTests(
            ActivityTimeframe timeframe)
            : base(timeframe)
        {
        }

        [Fact]
        public async Task CreatesYearEntry()
        {
            await TestExists(ActivityTimeframe.Year);
        }
    }
}