namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;

    using BookSleeve;

    public class CompositeUserActivityReport : UserActivityReport
    {
        public CompositeUserActivityReport(
            int db,
            string key,
            string operation,
            UserActivityReport left,
            UserActivityReport right)
            : base(db, key)
        {
            Validation.ValidateString(
                operation,
                ErrorMessages.CompositeUserActivityReport_Constructor_Operation_Required,
                "operation");

            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            if (left == null)
            {
                throw new ArgumentNullException("left");
            }

            Operation = operation;
            Left = left;
            Right = right;
        }

        protected string Operation { get; private set; }

        protected UserActivityReport Left { get; private set; }

        protected UserActivityReport Right { get; private set; }

        public override async Task<bool[]> Includes(params long[] users)
        {
            Validation.ValidateUsers(users);

            using (var connection = await ConnectionFactories.Open())
            {
                await PerformBitOperation(connection);

                return await InternalIncludes(connection, users);
            }
        }

        public override async Task<long> Count()
        {
            using (var connection = await ConnectionFactories.Open())
            {
                await PerformBitOperation(connection);

                return await InternalCount(connection);
            }
        }

        internal async Task<long> PerformBitOperation(
            RedisConnection connection)
        {
            var leftComposite = Left as CompositeUserActivityReport;
            var rightComposite = Right as CompositeUserActivityReport;

            var tasks = new List<Task>();

            if (leftComposite != null)
            {
                tasks.Add(leftComposite.PerformBitOperation(connection));
            }

            if (rightComposite != null)
            {
                tasks.Add(rightComposite.PerformBitOperation(connection));
            }

            if (tasks.Count > 0)
            {
                await Task.WhenAll(tasks);
            }

            var keys = new[] { Left.Key, Right.Key };

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
                    ErrorMessages.CompositeUserActivityReport_PerformBitOperation_Unsupported,
                    Operation));
        }
    }
}