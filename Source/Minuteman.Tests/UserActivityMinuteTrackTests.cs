namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityMinuteTrackTests : UserActivityHourTrackTests
    {
        public UserActivityMinuteTrackTests()
            : base(UserActivityDrilldownType.Minute)
        {
        }

        [Fact]
        public async Task CreatesMinuteEntry()
        {
            await TestExists();
        }
    }
}