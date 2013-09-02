namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    using BookSleeve;

    [CLSCompliant(false)]
    public class Subscription<TInfo> : ISubscription
        where TInfo : Info
    {
        private readonly RedisSubscriberConnection connection;
        private readonly string channel;
        private readonly Action<TInfo> action;

        private bool disposed;

        public Subscription(
            RedisSubscriberConnection connection,
            string channel,
            Action<TInfo> action)
        {
            if (connection == null)
            {
               throw new ArgumentNullException("connection");
            }

            Validation.ValidateString(
                channel,
                ErrorMessages.Subscription_Constructor_Channel_Required,
                "channel");

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.connection = connection;
            this.channel = channel;
            this.action = action;
        }

        ~Subscription()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual async Task Subscribe()
        {
            if (connection.State == RedisConnectionBase.ConnectionState.New)
            {
                await connection.Open();
            }

            await connection.Subscribe(
                channel,
                (key, data) =>
                    action(Info.Deserialize<TInfo>(
                        data)));
        }

        public virtual Task Unsubscribe()
        {
            return connection.Unsubscribe(channel);
        }

        protected virtual void DisposeCore()
        {
            connection.Dispose();
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                DisposeCore();
            }

            disposed = true;
        }
    }
}