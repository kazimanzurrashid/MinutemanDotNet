namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    using BookSleeve;

    [CLSCompliant(false)]
    public class Subscription<TInfo> : ISubscription<TInfo>
        where TInfo : Info
    {
        private readonly RedisSubscriberConnection connection;
        private readonly string prefix;

        private bool disposed;

        public Subscription(
            RedisSubscriberConnection connection,
            string prefix)
        {
            if (connection == null)
            {
               throw new ArgumentNullException("connection");
            }

            Validation.ValidateString(
                prefix,
                ErrorMessages.Subscription_Constructor_Prefix_Required,
                "prefix");

            this.connection = connection;
            this.prefix = prefix;
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

        public virtual async Task Subscribe(
            string eventName,
            Action<TInfo> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (connection.State == RedisConnectionBase.ConnectionState.New)
            {
                await connection.Open();
            }

            var channel = Channel(eventName);

            await connection.Subscribe(
                channel,
                (key, data) => action(Info.Deserialize<TInfo>(data)));
        }

        public virtual async Task Unsubscribe(string eventName)
        {
            var channel = Channel(eventName);

            await connection.Unsubscribe(channel);
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

        private string Channel(string eventName)
        {
            Validation.ValidateEventName(eventName);

            return prefix + eventName.ToUpperInvariant();
        }
    }
}