using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api
{
    public class MyClaimsTransformation : IClaimsTransformation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyClaimsTransformation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Task.FromResult(principal);
            }

            // Đọc cookie
            if (httpContext.Request.Cookies.TryGetValue("AclCookie", out var cookieValue))
            {
                var identity = (ClaimsIdentity)principal.Identity!;

                // Check để không add duplicate
                if (!identity.HasClaim(c => c.Type == "acl"))
                {
                    identity.AddClaim(new Claim("acl", cookieValue));
                }
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            var claimType = "myNewClaim";
            if (!principal.HasClaim(claim => claim.Type == claimType))
            {
                claimsIdentity.AddClaim(new Claim(claimType, "myClaimValue"));
            }

            principal.AddIdentity(claimsIdentity);
            return Task.FromResult(principal);
        }
    }
}
