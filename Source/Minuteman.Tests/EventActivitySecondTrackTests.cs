namespace Minuteman.Tests
{
    using System.Threading.Tasks;

    using Xunit;

    public class EventActivitySecondTrackTests : EventActivityMinuteTrackTests
    {
        public EventActivitySecondTrackTests() :
            base(ActivityDrilldown.Second)
        {
        }

        [Fact]
        public async Task CreatesSecondField()
        {
            await TestExists(ActivityDrilldown.Second);
        }
    }
}