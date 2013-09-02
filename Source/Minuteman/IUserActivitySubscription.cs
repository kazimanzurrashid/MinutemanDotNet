namespace Minuteman
{
    using System;
    using System.Threading.Tasks;

    public interface IUserActivitySubscription : IDisposable
    {
        Task Subscribe();

        Task Unsubscribe();
    }
}