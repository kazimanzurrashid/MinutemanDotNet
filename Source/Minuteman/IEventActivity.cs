namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IEventActivity : IActivity
    {
        Task Track(
            string eventName,
            ActivityDrilldown drilldown,
            DateTime timestamp);
    }
}