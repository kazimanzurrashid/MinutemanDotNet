namespace Minuteman.Tests
{
    using System;
    using System.Linq;

    using Xunit;

    public class DateTimeRangeTests
    {
        private static readonly DateTime Now = DateTime.UtcNow;

        [Fact]
        public void YearRange()
        {
            const ActivityDrilldown Drilldown = ActivityDrilldown.Year;
            Func<int, DateTime> func = n => Now.AddYears(n);
 
            // Positive
            Test(2, func, Drilldown);
            Test(7, func, Drilldown);
            Test(11, func, Drilldown);

            Assert.Equal(2, Now.Range(func(1), Drilldown).Count());
            Assert.Equal(7, Now.Range(func(6), Drilldown).Count());
            Assert.Equal(11, Now.Range(func(10), Drilldown).Count());

            // Negative
            Test(-2, func, Drilldown);
            Test(-7, func, Drilldown);
            Test(-11, func, Drilldown);

            // Same
            Test(0, func, Drilldown);
        }

        [Fact]
        public void MonthRange()
        {
            const ActivityDrilldown Drilldown = ActivityDrilldown.Month;
            Func<int, DateTime> func = n => Now.AddMonths(n);

            // Positive
            Test(13, func, Drilldown);
            Test(2, func, Drilldown);
            Test(23, func, Drilldown);

            // Negative
            Test(-13, func, Drilldown);
            Test(-2, func, Drilldown);
            Test(-23, func, Drilldown);

            // Same
            Test(0, func, Drilldown);
        }

        [Fact]
        public void DayRange()
        {
            const ActivityDrilldown Drilldown = ActivityDrilldown.Day;
            Func<int, DateTime> func = n => Now.AddDays(n);

            // Positive
            Test(27, func, Drilldown);
            Test(7, func, Drilldown);
            Test(143, func, Drilldown);

            // Negative
            Test(-27, func, Drilldown);
            Test(-7, func, Drilldown);
            Test(-143, func, Drilldown);

            // Same
            Test(0, func, Drilldown);
        }

        [Fact]
        public void HourRange()
        {
            const ActivityDrilldown Drilldown = ActivityDrilldown.Hour;
            Func<int, DateTime> func = n => Now.AddHours(n);

            // Positive
            Test(12, func, Drilldown);
            Test(3, func, Drilldown);
            Test(56, func, Drilldown);

            // Negative
            Test(-12, func, Drilldown);
            Test(-3, func, Drilldown);
            Test(-56, func, Drilldown);

            // Same
            Test(0, func, Drilldown);
        }

        private static void Test(
            int interval,
            Func<int, DateTime> func,
            ActivityDrilldown drilldown)
        {
            Assert.Equal(
                Math.Abs(interval) + 1,
                Now.Range(func(interval), drilldown).Count());
        }
    }
}