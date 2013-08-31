namespace Minuteman
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using BookSleeve;

    public class CompositeUsersResult : UsersResult
    {
        public CompositeUsersResult(
            int db,
            string key,
            string operation,
            string leftKey,
            string rightKey)
            : base(db, key)
        {
            Validation.ValidateString(
                operation,
                ErrorMessages.CompositeUsersResult_Constructor_Operation_Required,
                "operation");

            Validation.ValidateString(
                leftKey,
                ErrorMessages.CompositeUsersResult_Constructor_LeftKey_Required,
                "leftKey");

            Validation.ValidateString(
                rightKey,
                ErrorMessages.CompositeUsersResult_Constructor_RightKey_Required,
                "rightKey");

            Operation = operation;
            LeftKey = leftKey;
            RightKey = rightKey;
        }

        protected string LeftKey { get; private set; }

        protected string RightKey { get; private set; }

        protected string Operation { get; private set; }

        public override async Task<bool[]> Includes(params long[] users)
        {
            Validation.ValidateUsers(users);

            using (var connection = await UserActivity.OpenConnection())
            {
                await PerformBitOperation(connection);

                return await InternalIncludes(connection, users);
            }
        }

        public override async Task<long> Count()
        {
            using (var connection = await UserActivity.OpenConnection())
            {
                await PerformBitOperation(connection);

                return await InternalCount(connection);
            }
        }

        internal async Task<long> PerformBitOperation(
            RedisConnection connection)
        {
            var keys = new[] { LeftKey, RightKey };

            if ("AND".Equals(Operation, StringComparison.Ordinal))
            {
                return await connection.Strings.BitwiseAnd(Db, Key, keys);
            }
            
            if ("OR".Equals(Operation, StringComparison.Ordinal))
            {
                return await connection.Strings.BitwiseOr(Db, Key, keys);
            }

            if ("XOR".Equals(Operation, StringComparison.Ordinal))
            {
                return await connection.Strings.BitwiseXOr(Db, Key, keys);
            }

            throw new InvalidOperationException(
                string.Format(
                    CultureInfo.CurrentUICulture,
                    ErrorMessages.CompositeUsersResult_PerformBitOperation_Unsupported,
                    Operation));
        }
    }
}