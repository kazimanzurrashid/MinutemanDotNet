namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    using BookSleeve;

    public static class ConnectionFactory
    {
        private static Func<RedisConnection> factory = () =>
            new RedisConnection("localhost");

        private static Func<RedisSubscriberConnection> subscriberFactory = () =>
            new RedisSubscriberConnection("localhost");

        public static Func<RedisConnection> Factory
        {
            get { return factory; }

            set { factory = value; }
        }

        public static Func<RedisSubscriberConnection> SubscriberFactory
        {
            get { return subscriberFactory; }

            set { subscriberFactory = value; }
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