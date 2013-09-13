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
            const ActivityTimeframe Timeframe = ActivityTimeframe.Year;
            Func<int, DateTime> func = n => Now.AddYears(n);
 
            // Positive
            Test(2, func, Timeframe);
            Test(7, func, Timeframe);
            Test(11, func, Timeframe);

            Assert.Equal(2, Now.Range(func(1), Timeframe).Count());
            Assert.Equal(7, Now.Range(func(6), Timeframe).Count());
            Assert.Equal(11, Now.Range(func(10), Timeframe).Count());

            // Negative
            Test(-2, func, Timeframe);
            Test(-7, func, Timeframe);
            Test(-11, func, Timeframe);

            // Same
            Test(0, func, Timeframe);
        }

        [Fact]
        public void MonthRange()
        {
            const ActivityTimeframe Timeframe = ActivityTimeframe.Month;
            Func<int, DateTime> func = n => Now.AddMonths(n);

            // Positive
            Test(13, func, Timeframe);
            Test(2, func, Timeframe);
            Test(23, func, Timeframe);

            // Negative
            Test(-13, func, Timeframe);
            Test(-2, func, Timeframe);
            Test(-23, func, Timeframe);

            // Same
            Test(0, func, Timeframe);
        }

        [Fact]
        public void DayRange()
        {
            const ActivityTimeframe Timeframe = ActivityTimeframe.Day;
            Func<int, DateTime> func = n => Now.AddDays(n);

            // Positive
            Test(27, func, Timeframe);
            Test(7, func, Timeframe);
            Test(143, func, Timeframe);

            // Negative
            Test(-27, func, Timeframe);
            Test(-7, func, Timeframe);
            Test(-143, func, Timeframe);

            // Same
            Test(0, func, Timeframe);
        }

        [Fact]
        public void HourRange()
        {
            const ActivityTimeframe Timeframe = ActivityTimeframe.Hour;
            Func<int, DateTime> func = n => Now.AddHours(n);

            // Positive
            Test(12, func, Timeframe);
            Test(3, func, Timeframe);
            Test(56, func, Timeframe);

            // Negative
            Test(-12, func, Timeframe);
            Test(-3, func, Timeframe);
            Test(-56, func, Timeframe);

            // Same
            Test(0, func, Timeframe);
        }

        private static void Test(
            int interval,
            Func<int, DateTime> func,
            ActivityTimeframe timeframe)
        {
            Assert.Equal(
                Math.Abs(interval) + 1,
                Now.Range(func(interval), timeframe).Count());
        }
    }
}