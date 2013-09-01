namespace Minuteman
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    internal static class DateTimeExtensions
    {
        private static readonly Func<DateTime, DateTime, bool> 
            Greater = (d1, d2) => d1 > d2;

        private static readonly Func<DateTime, DateTime, bool>
            Lesser = (d1, d2) => d1 < d2;

        public static IEnumerable<DateTime> Range(
            this DateTime instance,
            DateTime other,
            ActivityDrilldown drilldown)
        {
            var backward = instance > other;
            var increment = backward ? -1 : 1;
            var outer = backward ? Greater : Lesser;
            var inner = backward ? Lesser : Greater;

            yield return instance;

            var counter = instance;

            while (outer(counter, other))
            {
                switch (drilldown)
                {
                    case ActivityDrilldown.Hour:
                        {
                            counter = counter.AddHours(increment);
                            break;
                        }

                    case ActivityDrilldown.Day:
                        {
                            counter = counter.AddDays(increment);
                            break;
                        }

                    case ActivityDrilldown.Month:
                        {
                            counter = counter.AddMonths(increment);
                            break;
                        }

                    case ActivityDrilldown.Minute:
                        {
                            counter = counter.AddMinutes(increment);
                            break;
                        }

                    case ActivityDrilldown.Year:
                        {
                            counter = counter.AddYears(increment);
                            break;
                        }

                    case ActivityDrilldown.Second:
                        {
                            counter = counter.AddSeconds(increment);
                            break;
                        }
                }

                if (inner(counter, other))
                {
                    continue;
                }

                yield return counter;
            }
        }

        public static string FormatYear(this DateTime instance)
        {
            return Format(instance.Year, "d4");
        }

        public static string FormatMonth(this DateTime instance)
        {
            return Format(instance.Month);
        }

        public static string FormatDay(this DateTime instance)
        {
            return Format(instance.Day);
        }

        public static string FormatHour(this DateTime instance)
        {
            return Format(instance.Hour);
        }

        public static string FormatMinute(this DateTime instance)
        {
            return Format(instance.Minute);
        }

        public static string FormatSecond(this DateTime instance)
        {
            return Format(instance.Second);
        }

        private static string Format(int value, string format = "d2")
        {
            return value.ToString(format, CultureInfo.InvariantCulture);
        }
    }
}