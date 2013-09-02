namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    using BookSleeve;

    [CLSCompliant(false)]
    public class UserActivitySubscription : IUserActivitySubscription
    {
        private readonly RedisSubscriberConnection connection;
        private readonly string channel;
        private readonly Action<UserActivitySubscriptionInfo> action;

        private bool disposed;
 
        public UserActivitySubscription(
            RedisSubscriberConnection connection,
            string channel,
            Action<UserActivitySubscriptionInfo> action)
        {
            if (connection == null)
            {
               throw new ArgumentNullException("connection");
            }

            Validation.ValidateString(
                channel,
                ErrorMessages.UserActivitySubscription_Constructor_Channel_Required,
                "channel");

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            this.connection = connection;
            this.channel = channel;
            this.action = action;
        }

        ~UserActivitySubscription()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task Subscribe()
        {
            if (connection.State == RedisConnectionBase.ConnectionState.New)
            {
                await connection.Open();
            }

            await connection.Subscribe(
                channel,
                (key, data) =>
                    action(UserActivitySubscriptionInfo.Deserialize(data)));
        }

        public Task Unsubscribe()
        {
            return connection.Unsubscribe(channel);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                connection.Dispose();
            }

            disposed = true;
        }
    }
}