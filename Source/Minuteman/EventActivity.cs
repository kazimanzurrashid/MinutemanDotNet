namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventActivity : Activity, IEventActivity
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

        public virtual async Task Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp)
        {
            Validation.ValidateEventName(eventName);

            var key = GenerateKey(eventName, drilldown.ToString());
            var eventsKey = GenerateKey();
            var fields = GenerateTimeframeFields(drilldown, timestamp).ToList();
            var db = Settings.Db;

            using (var connection = await ConnectionFactory.Open())
            {
                var tasks = fields
                    .Select(field => connection.Hashes
                        .Increment(db, key, field))
                    .Cast<Task>()
                    .ToList();

                tasks.Add(connection.Sets.Add(db, eventsKey, eventName));

                await Task.WhenAll(tasks);
            }
        }

        public virtual async Task<long[]> Counts(
            string eventName,
            DateTime startTimestamp,
            DateTime endTimestamp,
            ActivityDrilldown drilldown)
        {
            Validation.ValidateEventName(eventName);

            var dates = startTimestamp.Range(endTimestamp, drilldown);
            var key = GenerateKey(eventName, drilldown.ToString());

            var fields = dates.Select(d =>
                    GenerateTimeframeFields(drilldown, d)
                        .ElementAt((int)drilldown))
                .ToArray();

            string[] values;

            using (var connection = await ConnectionFactory.Open())
            {
                values = await connection.Hashes.GetString(
                    Settings.Db,
                    key,
                    fields);
            }

            var result = values.Select(v =>
                    string.IsNullOrWhiteSpace(v) ? 0L : long.Parse(v))
                .ToArray();

            return result;
        }

        internal IEnumerable<string> GenerateTimeframeFields(
            ActivityDrilldown drilldown,
            DateTime timestamp)
        {
            yield return timestamp.FormatYear();

            var separator = Settings.KeySeparator;
            var type = (int)drilldown;

            if (type > (int)ActivityDrilldown.Year)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth();
            }

            if (type > (int)ActivityDrilldown.Month)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay();
            }

            if (type > (int)ActivityDrilldown.Day)
            {
                yield return timestamp.FormatYear() +
                    separator +
                    timestamp.FormatMonth() +
                    separator +
                    timestamp.FormatDay() +
                    separator +
                    timestamp.FormatHour();
            }

            if (type > (int)ActivityDrilldown.Hour)
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

            if (type > (int)ActivityDrilldown.Minute)
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