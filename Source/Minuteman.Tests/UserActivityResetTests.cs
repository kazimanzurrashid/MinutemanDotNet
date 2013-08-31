namespace Minuteman.Tests
{
    using System;
    using System.Threading.Tasks;

    using Xunit;

    public class UserActivityResetTests : IDisposable
    {
        private readonly UserActivity userActivity;

        public UserActivityResetTests()
        {
            userActivity = new UserActivity(new ActivitySettings(1));
            userActivity.Reset().Wait();
        }

        public void Dispose()
        {
            userActivity.Reset().Wait();
        }

        [Fact]
        public async Task RemovesEverythingsThatStartsWithKeyPrefix()
        {
            using (var connection = await ConnectionFactory.Open())
            {
                await connection.Sets.Add(
                    userActivity.Settings.Db,
                    userActivity.GenerateKey("sets"),
                    "foo");

                await connection.Hashes.Set(
                    userActivity.Settings.Db,
                    userActivity.GenerateKey("hash"),
                    "bar",
                    "baz");

                await connection.Lists.AddFirst(
                    userActivity.Settings.Db,
                    userActivity.GenerateKey("list"),
                    "qux");

                await connection.Strings.Set(
                    userActivity.Settings.Db,
                    userActivity.GenerateKey("key"),
                    "quux");
            }

            var count = await userActivity.Reset();

            Assert.Equal(4, count);
        }
    }
}