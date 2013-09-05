namespace Minuteman
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using BookSleeve;

    public class UserActivityReport
    {
        public UserActivityReport(int db, string key)
        {
            Validation.ValidateDb(db);
            Validation.ValidateString(
                key,
                ErrorMessages.UserActivityReport_Constructor_Required,
                "key");

            Db = db;
            Key = key;
        }

        protected internal int Db { get; private set; }

        protected internal string Key { get; private set; }

        public static UserActivityReport BitwiseAnd(
            UserActivityReport left,
            UserActivityReport right)
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

            return new CompositeUserActivityReport(
                left.Db,
                key,
                "AND",
                left,
                right);
        }

        public static UserActivityReport BitwiseOr(
            UserActivityReport left,
            UserActivityReport right)
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

            return new CompositeUserActivityReport(
                left.Db,
                key,
                "OR",
                left,
                right);
        }

        public static UserActivityReport Xor(
            UserActivityReport left,
            UserActivityReport right)
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

            return new CompositeUserActivityReport(
                left.Db,
                key,
                "XOR",
                left,
                right);
        }

        public static UserActivityReport operator &(
            UserActivityReport left,
            UserActivityReport right)
        {
            return BitwiseAnd(left, right);
        }

        public static UserActivityReport operator |(
            UserActivityReport left,
            UserActivityReport right)
        {
            return BitwiseOr(left, right);
        }

        public static UserActivityReport operator ^(
            UserActivityReport left,
            UserActivityReport right)
        {
            return Xor(left, right);
        }

        public virtual async Task<bool[]> Includes(params long[] users)
        {
            Validation.ValidateUsers(users);

            using (var connection = await ConnectionFactories.Open())
            {
                return await InternalIncludes(connection, users);
            }
        }

        public virtual async Task<long> Count()
        {
            using (var connection = await ConnectionFactories.Open())
            {
                return await InternalCount(connection);
            }
        }

        public virtual async Task<bool> Remove()
        {
            bool result;

            using (var connection = await ConnectionFactories.Open())
            {
                result = await connection.Keys.Remove(Db, Key);
            }

            return result;
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