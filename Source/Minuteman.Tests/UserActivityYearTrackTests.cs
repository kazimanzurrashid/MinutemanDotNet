namespace Minuteman.Tests
{
    using System.Linq;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityYearTrackTests : UserActivityTrackTests
    {
        public UserActivityYearTrackTests() :
            this(ActivityDrilldown.Year)
        {
        }

        protected UserActivityYearTrackTests(
            ActivityDrilldown drilldown) : base(drilldown)
        {
        }

        [Fact]
        public async Task CreatesYearEntry()
        {
            await TestExists();
        }

        protected async Task TestExists()
        {
            var key = UserActivity.GenerateEventTimeframeKeys(
                EventName,
                UserActivity.Settings.Drilldown,
                Timestamp)
                .ElementAt((int)UserActivity.Settings.Drilldown);

            using (var connection = await UserActivity.OpenConnection())
            {
                var result = await connection.Strings.Get(
                    UserActivity.Settings.Db,
                    key);

                Assert.NotNull(result);
            }
        }
    }
}