namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface ISubscription : IDisposable
    {
        Task Subscribe();

        Task Unsubscribe();
    }
}