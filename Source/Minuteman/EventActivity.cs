namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventActivity : Activity<EventActivitySubscriptionInfo>,
        IEventActivity
    {
        private static readonly string EventsKeyName =
            typeof(EventActivity).Name;

        public EventActivity() : this(new ActivitySettings())
        {
        }

        public EventActivity(ActivitySettings settings) : base(settings)
        {
        }

        protected override string Prefix
        {
            get { return EventsKeyName; }
        }

        public virtual async Task<long> Track(
            string eventName,
            ActivityTimeframe timeframe,
            DateTime timestamp,
            bool publishable)
        {
            // There are three tasks that we have to do here.
            // First, we have to maintain a list of events that has 
            // been tracked, so that, the same event list can be 
            // returned in the EventName method. Next. we also have to
            // maintain another list that is for time frames of the event, 
            // please note that we are only going to maintain
            // the explicit timeframes, the Timeframes method returns the 
            // explicit tracked timeframes for a given event name.
            // Second, increase the count for the matching timeframe. And at
            // last publish the event to redis so that the subscriber can be 
            // notified.
            Validation.ValidateEventName(eventName);

            var eventsKey = GenerateKey();
            var publishedEventsKey = eventsKey +
                Settings.KeySeparator +
                "published";

            var key = GenerateKey(eventName, timeframe.ToString());
            var fields = GenerateTimeframeFields(timeframe, timestamp).ToList();

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

                var fieldTasks = fields
                    .Select(field => connection.Hashes
                        .Increment(db, key, field))
                    .ToList();

                var counts = await Task.WhenAll(fieldTasks);
                var count = counts.ElementAt((int)timeframe);

                if (!publishable)
                {
                    return count;
                }

                var channel = eventsKey +
                    Settings.KeySeparator +
                    eventName.ToUpperInvariant();

                var payload = new EventActivitySubscriptionInfo
                {
                    EventName = eventName,
                    Timestamp = timestamp,
                    Timeframe = timeframe,
                    Count = counts.ElementAt((int)timeframe)
                }.Serialize();

                await connection.Publish(channel, payload);

                return count;
            }
        }

        public virtual async Task<long[]> Counts(
            string eventName,
            DateTime startTimestamp,
            DateTime endTimestamp,
            ActivityTimeframe timeframe)
        {
            Validation.ValidateEventName(eventName);

            var dates = startTimestamp.Range(endTimestamp, timeframe);
            var key = GenerateKey(eventName, timeframe.ToString());

            var fields = dates.Select(d =>
                    GenerateTimeframeFields(timeframe, d)
                        .ElementAt((int)timeframe))
                .ToArray();

            string[] values;

            using (var connection = await ConnectionFactories.Open())
            {
                values = await connection.Hashes.GetString(
                    Settings.Db,
                    key,
                    fields);
            }

            var result = values.Select(value =>
                    value == null ? 0L : long.Parse(value))
                .ToArray();

            return result;
        }

        internal IEnumerable<string> GenerateTimeframeFields(
            ActivityTimeframe timeframe,
            DateTime timestamp)
        {
            yield return timestamp.FormatYear();

            var separator = Settings.KeySeparator;
            var type = (int)timeframe;

            if (type > (int)ActivityTimeframe.Year)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth();
            }

            if (type > (int)ActivityTimeframe.Month)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay();
            }

            if (type > (int)ActivityTimeframe.Day)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay() +
                    separator +
                    timestamp.FormatHour();
            }

            if (type > (int)ActivityTimeframe.Hour)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay() +
                    separator +
                    timestamp.FormatHour() +
                    separator +
                    timestamp.FormatMinute();
            }

            if (type > (int)ActivityTimeframe.Minute)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay() +
                    separator +
                    timestamp.FormatHour() +
                    separator +
                    timestamp.FormatMinute() +
                    separator +
                    timestamp.FormatSecond();
            }
        }
    }
}