namespace Minuteman
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public static class UserActivityReportExtensions
    {
        public static async Task<bool> Contains(
            this UserActivityReport instance,
            long user)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            var result = await instance.Includes(user);

            return result.First();
        }
    }
}