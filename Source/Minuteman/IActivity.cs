namespace Minuteman
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IActivity
    {
        ActivitySettings Settings { get; }

        Task<IEnumerable<string>> EventNames();

        Task<long> Reset();
    }
}