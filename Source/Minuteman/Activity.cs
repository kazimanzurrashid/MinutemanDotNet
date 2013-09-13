namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Activity<TInfo> : IActivity<TInfo>
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

        public virtual async Task<IEnumerable<string>> EventNames(
            bool onlyPublished)
        {
            var eventsKey = GenerateKey();

            if (onlyPublished)
            {
                eventsKey += Settings.KeySeparator + "published";
            }

            string[] members;

            using (var connection = await ConnectionFactories.Open())
            {
                members = await connection.Sets.GetAllString(
                    Settings.Db,
                    eventsKey);
            }

            var result = members.Select(RemoveKeyPrefix)
                .OrderBy(n => n)
                .ToList();

            return result;
        }

        public virtual async Task<IEnumerable<ActivityTimeframe>> Timeframes(
            string eventName)
        {
            Validation.ValidateEventName(eventName);

            var key = GenerateKey() +
                Settings.KeySeparator +
                eventName;

            string[] members;

            using (var connection = await ConnectionFactories.Open())
            {
                members = await connection.Sets.GetAllString(Settings.Db, key);
            }

            var result = members.Select(m => 
                (ActivityTimeframe)Enum.Parse(typeof(ActivityTimeframe), m))
                .ToList();

            return result;
        }

        public virtual async Task<long> Reset()
        {
            var wildcard = GenerateKey() + "*";
            var db = Settings.Db;
            long result = 0;

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

        public virtual ISubscription<TInfo> CreateSubscription()
        {
            var prefix = GenerateKey() + Settings.KeySeparator;

            var subscription = new Subscription<TInfo>(
                ConnectionFactories.SubscriberFactory(),
                prefix);

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