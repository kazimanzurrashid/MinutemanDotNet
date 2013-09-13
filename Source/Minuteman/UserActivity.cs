namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UserActivity : Activity<UserActivitySubscriptionInfo>,
        IUserActivity
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
            ActivityTimeframe timeframe,
            DateTime timestamp,
            bool publishable,
            params long[] users)
        {
            Validation.ValidateEventName(eventName);
            Validation.ValidateUsers(users);

            var eventsKey = GenerateKey();
            var publishedEventsKey = eventsKey +
                Settings.KeySeparator +
                "published";

            string channel = null;
            byte[] payload = null;

            if (publishable)
            {
                channel = eventsKey +
                    Settings.KeySeparator +
                    eventName.ToUpperInvariant();

                payload = new UserActivitySubscriptionInfo
                {
                    EventName = eventName,
                    Timestamp = timestamp,
                    Timeframe = timeframe,
                    Users = users
                }.Serialize();
            }

            var timeframeKeys = GenerateEventTimeframeKeys(
                eventName,
                timeframe,
                timestamp).ToList();

            var db = Settings.Db;

            using (var connection = await ConnectionFactories.Open())
            {
                var eventTasks = new List<Task>
                {
                    connection.Sets.Add(db, eventsKey, eventName),
                    connection.Sets.Add(
                        db,
                        eventsKey + Settings.KeySeparator + eventName,
                        timeframe.ToString())
                };

                if (publishable)
                {
                    eventTasks.Add(
                        connection.Sets.Add(
                            db,
                            publishedEventsKey,
                            eventName));
                }

                await Task.WhenAll(eventTasks);

                var bitTasks = new List<Task>();

                foreach (var timeframeKey in timeframeKeys)
                {
                    bitTasks.AddRange(users.Select(user =>
                        connection.Strings.SetBit(
                            db,
                            timeframeKey,
                            user,
                            true)));
                }

                await Task.WhenAll(bitTasks);

                if (publishable)
                {
                    await connection.Publish(channel, payload);
                }
            }
        }

        public virtual UserActivityReport Report(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp)
        {
            Validation.ValidateEventName(eventName);

            var eventKey = GenerateEventTimeframeKeys(
                eventName, timeframe, timestamp).ElementAt((int)timeframe);

            return new UserActivityReport(Settings.Db, eventKey);
        }

        internal IEnumerable<string> GenerateEventTimeframeKeys(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp)
        {
            yield return GenerateKey(
                eventName,
                timestamp.FormatYear());

            var type = (int)timeframe;

            if (type > (int)ActivityTimeframe.Year)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth());
            }

            if (type > (int)ActivityTimeframe.Month)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay());
            }

            if (type > (int)ActivityTimeframe.Day)
            {
                yield return GenerateKey(
                    eventName, 
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay(),
                    timestamp.FormatHour());
            }

            if (type > (int)ActivityTimeframe.Hour)
            {
                yield return GenerateKey(
                    eventName,
                    timestamp.FormatYear(),
                    timestamp.FormatMonth(),
                    timestamp.FormatDay(),
                    timestamp.FormatHour(),
                    timestamp.FormatMinute());
            }

            if (type > (int)ActivityTimeframe.Minute)
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