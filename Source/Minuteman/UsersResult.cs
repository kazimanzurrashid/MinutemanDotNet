namespace Minuteman
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BookSleeve;

    public class UsersResult
    {
        public UsersResult(int db, string key)
        {
            Validation.ValidateDb(db);
            Validation.ValidateString(
                key,
                ErrorMessages.UsersResult_Constructor_Required,
                "key");

            Db = db;
            Key = key;
        }

        protected int Db { get; private set; }

        protected string Key { get; private set; }

        public static UsersResult BitwiseAnd(UsersResult left, UsersResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var key = left.Key + "&" + right.Key;

            return new CompositeUsersResult(
                left.Db,
                key,
                "AND",
                left.Key,
                right.Key);
        }

        public static UsersResult BitwiseOr(UsersResult left, UsersResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var key = left.Key + "|" + right.Key;

            return new CompositeUsersResult(
                left.Db,
                key,
                "OR",
                left.Key,
                right.Key);
        }

        public static UsersResult Xor(UsersResult left, UsersResult right)
        {
            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (right == null)
            {
                throw new ArgumentNullException("right");
            }

            var key = left.Key + "^" + right.Key;

            return new CompositeUsersResult(
                left.Db,
                key,
                "XOR",
                left.Key,
                right.Key);
        }

        public static UsersResult operator &(
            UsersResult left,
            UsersResult right)
        {
            return BitwiseAnd(left, right);
        }

        public static UsersResult operator |(
            UsersResult left,
            UsersResult right)
        {
            return BitwiseOr(left, right);
        }

        public static UsersResult operator ^(
            UsersResult left,
            UsersResult right)
        {
            return Xor(left, right);
        }

        public virtual async Task<bool[]> Includes(params long[] users)
        {
            Validation.ValidateUsers(users);

            using (var connection = await UserActivity.OpenConnection())
            {
                return await InternalIncludes(connection, users);
            }
        }

        public virtual async Task<long> Count()
        {
            using (var connection = await UserActivity.OpenConnection())
            {
                return await InternalCount(connection);
            }
        }

        internal async Task<bool[]> InternalIncludes(
            RedisConnection connection,
            params long[] users)
        {
            var result = await Task.WhenAll(
                users.Select(user =>
                    connection.Strings.GetBit(Db, Key, user)));

            return result;
        }

        internal async Task<long> InternalCount(RedisConnection connection)
        {
            var result = await connection.Strings.CountSetBits(Db, Key);

            return result;
        }
    }
}