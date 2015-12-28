using System.Threading;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Infrastructure
{
    public class AspNetUserIdProvider : IUserIdProvider
    {
        public string GetUserId()
        {
            return Thread.CurrentPrincipal.Identity.GetUserId();
        }
    }
}