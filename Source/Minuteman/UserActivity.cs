namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserActivity : Activity, IUserActivity
    {
        private static readonly string EventsKeyName =
            typeof(UserActivity).Name;

        public UserActivity() : this(new ActivitySettings())
        {
        }

        public UserActivity(ActivitySettings settings) : base(settings)
        {
        }

        protected override string Prefix
        {
            get { return EventsKeyName; }
        }

        public virtual async Task Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp,
            bool publishable,
            params long[] users)
        {
            Validation.ValidateEventName(eventName);
            Validation.ValidateUsers(users);

            string channel = null;
            byte[] payload = null;

            var timeframeKeys = GenerateEventTimeframeKeys(
                eventName,
                drilldown,
                timestamp).ToList();

            var eventsKey = GenerateKey();

            var db = Settings.Db;
            var tasks = new List<Task>();

            if (publishable)
            {
                channel = eventsKey +
                    Settings.KeySeparator +
                    eventName.ToUpperInvariant();

                payload = new UserActivitySubscriptionInfo
                {
                    EventName = eventName,
                    Timestamp = timestamp,
                    Users = users
                }.Serialize();
            }

            using (var connection = await ConnectionFactory.Open())
            {
                foreach (var timeframeKey in timeframeKeys)
                {
                    tasks.AddRange(users.Select(user =>
                        connection.Strings.SetBit(
                            db,
                            timeframeKey,
                            user,
                            true)));
                }

                tasks.Add(connection.Sets.Add(db, eventsKey, eventName));

                await Task.WhenAll(tasks);

                if (publishable)
                {
                    await connection.Publish(channel, payload);
                }
            }
        }

        public virtual UserActivityReport Report(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp)
        {
            Validation.ValidateEventName(eventName);

            var eventKey = GenerateEventTimeframeKeys(
                eventName, drilldown, timestamp).ElementAt((int)drilldown);

            return new UserActivityReport(Settings.Db, eventKey);
        }

        public virtual IUserActivitySubscription CreateSubscription(
            string eventName,
            Action<UserActivitySubscriptionInfo> action)
        {
            Validation.ValidateEventName(eventName);

            var channel = GenerateKey() +
                Settings.KeySeparator +
                eventName.ToUpperInvariant();

            var subscription = new UserActivitySubscription(
                ConnectionFactory.SubscriberFactory(),
                channel,
                action);

            return subscription;
        }

        internal IEnumerable<string> GenerateEventTimeframeKeys(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp)
        {
            yield return GenerateKey(
                eventName,
                timestamp.FormatYear());

            var type = (int)drilldown;

            if (type > (int)ActivityDrilldown.Year)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth());
            }

            if (type > (int)ActivityDrilldown.Month)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay());
            }

            if (type > (int)ActivityDrilldown.Day)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay(),
                    timestamp.FormatHour());
            }

            if (type > (int)ActivityDrilldown.Hour)
            {
                yield return GenerateKey(
                    eventName,
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay(),
                    timestamp.FormatHour(),
                    timestamp.FormatMinute());
            }

            if (type > (int)ActivityDrilldown.Minute)
            {
                yield return GenerateKey(
                    eventName,
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay(),
                    timestamp.FormatHour(),
                    timestamp.FormatMinute(),
                    timestamp.FormatSecond());
            }
        }
    }
}