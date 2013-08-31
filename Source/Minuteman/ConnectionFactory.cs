namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    using BookSleeve;

    public static class ConnectionFactory
    {
        private static Func<RedisConnection> factory = () =>
            new RedisConnection("localhost"); 

        public static Func<RedisConnection> Factory
        {
            get { return factory; }

            set { factory = value; }
        }

        internal static async Task<RedisConnection> Open()
        {
            RedisConnection connection = null;

            try
            {
                connection = Factory();

                if (connection.State == RedisConnectionBase.ConnectionState
                    .New)
                {
                    await connection.Open();
                }
            }
            catch (Exception)
            {
                if (connection != null)
                {
                    connection.Dispose();
                }

                throw;
            }

            return connection;
        }
    }
}