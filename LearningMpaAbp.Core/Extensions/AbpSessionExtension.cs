using System.Linq;
using System.Security.Claims;
using Abp.Configuration.Startup;
using Abp.Runtime.Session;

namespace LearningMpaAbp.Extensions
{
    public class AbpSessionExtension : ClaimsAbpSession, IAbpSessionExtension
    {
        public AbpSessionExtension(IMultiTenancyConfig multiTenancy) : base(multiTenancy)
        {
        }

        public string Email => GetClaimValue(ClaimTypes.Email);

        private string GetClaimValue(string claimType)
        {
            var claimsPrincipal = PrincipalAccessor.Principal;

            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }
    }
}