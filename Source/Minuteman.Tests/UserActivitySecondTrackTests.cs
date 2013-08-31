namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivitySecondTrackTests : UserActivityMinuteTrackTests
    {
        public UserActivitySecondTrackTests()
            : base(ActivityDrilldown.Second)
        {
        }

        [Fact]
        public async Task CreatesSecondEntry()
        {
            await TestExists(ActivityDrilldown.Second);
        }
    }
}