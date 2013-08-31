namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMinuteTrackTests : UserActivityHourTrackTests
    {
        public UserActivityMinuteTrackTests()
            : base(ActivityDrilldownType.Minute)
        {
        }

        [Fact]
        public async Task CreatesMinuteEntry()
        {
            await TestExists();
        }
    }
}