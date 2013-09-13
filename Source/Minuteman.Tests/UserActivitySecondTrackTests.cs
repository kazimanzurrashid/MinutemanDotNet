namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivitySecondTrackTests : UserActivityMinuteTrackTests
    {
        public UserActivitySecondTrackTests()
            : base(ActivityTimeframe.Second)
        {
        }

        [Fact]
        public async Task CreatesSecondEntry()
        {
            await TestExists(ActivityTimeframe.Second);
        }
    }
}