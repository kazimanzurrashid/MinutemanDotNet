namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface ISubscription<out TInfo> : IDisposable where TInfo : Info
    {
        Task Subscribe(string eventName, Action<TInfo> action);

        Task Unsubscribe(string eventName);
    }
}