namespace Minuteman
{
    using System.Threading.Tasks;

    using BookSleeve;

    public class NegatedUsersResult : UsersResult
    {
        public NegatedUsersResult(int db, string key, string otherKey)
            : base(db, key)
        {
            Validation.ValidateString(
                otherKey,
                ErrorMessages.NegatedUsersResult_Constructor_Required,
                "otherKey");

            OtherKey = otherKey;
        }

        public string OtherKey { get; private set; }

        public override async Task<bool[]> Includes(params long[] users)
        {
            Validation.ValidateUsers(users);

            using (var connection = await UserActivity.OpenConnection())
            {
                await PerformNotOperation(connection);

                return await InternalIncludes(connection, users);
            }
        }

        public override async Task<long> Count()
        {
            using (var connection = await UserActivity.OpenConnection())
            {
                await PerformNotOperation(connection);

                return await InternalCount(connection);
            }
        }

        private async Task<long> PerformNotOperation(
            RedisConnection connection)
        {
            return await connection.Strings.BitwiseNot(Db, Key, OtherKey);
        }
    }
}