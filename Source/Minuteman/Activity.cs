namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Activity<TInfo> : IActivity
        where TInfo : Info
    {
        protected Activity(ActivitySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Settings = settings;
        }

        public ActivitySettings Settings { get; private set; }

        protected abstract string Prefix { get; }

        public virtual async Task<IEnumerable<string>> EventNames()
        {
            var eventsKey = GenerateKey();
            var db = Settings.Db;
            string[] names;

            using (var connection = await ConnectionFactories.Open())
            {
                names = await connection.Sets.GetAllString(db, eventsKey);
            }

            var result = names.Select(RemoveKeyPrefix);

            return result;
        }

        public virtual async Task<long> Reset()
        {
            long result = 0;
            var wildcard = GenerateKey() + "*";
            var db = Settings.Db;

            using (var connection = await ConnectionFactories.Open())
            {
                var keys = await connection.Keys.Find(db, wildcard);

                if (keys.Any())
                {
                    result = await connection.Keys.Remove(db, keys);
                }
            }

            return result;
        }

        public virtual ISubscription CreateSubscription(
            string eventName,
            Action<TInfo> action)
        {
            Validation.ValidateEventName(eventName);

            var channel = GenerateKey() +
                Settings.KeySeparator +
                eventName.ToUpperInvariant();

            var subscription = new Subscription<TInfo>(
                ConnectionFactories.SubscriberFactory(),
                channel,
                action);

            return subscription;
        }

        protected internal string GenerateKey(params string[] parts)
        {
            var prefix = Settings.KeyPrefix + Settings.KeySeparator + Prefix;

            if (!parts.Any())
            {
                return prefix;
            }

            var key = prefix +
                Settings.KeySeparator +
                string.Join(
                    Settings.KeySeparator,
                    parts.Select(part => part.ToUpperInvariant()));

            return key;
        }

        private string RemoveKeyPrefix(string value)
        {
            var fullPrefix = GenerateKey(Prefix);
            var index = value.IndexOf(fullPrefix, StringComparison.Ordinal);

            var result = index < 0 ? value : value.Substring(index);

            return result;
        }
    }
}