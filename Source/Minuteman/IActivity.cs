namespace Minuteman
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
   
    public interface IActivity<out TInfo> where TInfo : Info
    {
        ActivitySettings Settings { get; }

        Task<IEnumerable<string>> EventNames(bool onlyPublished);

        Task<IEnumerable<ActivityTimeframe>> Timeframes(string eventName);

        Task<long> Reset();

        ISubscription<TInfo> CreateSubscription();
    }
}