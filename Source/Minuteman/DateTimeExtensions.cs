namespace Minuteman
{
    using System;
    using System.Globalization;

    internal static class DateTimeExtensions
    {
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