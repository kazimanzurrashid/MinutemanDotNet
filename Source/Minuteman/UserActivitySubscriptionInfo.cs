namespace Minuteman
{
    using System.Collections.Generic;

    public class UserActivitySubscriptionInfo : Info
    {
       public IEnumerable<long> Users { get; set; }
    }
}