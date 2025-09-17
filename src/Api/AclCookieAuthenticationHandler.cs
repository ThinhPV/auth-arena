using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Api
{
    public class AclCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AclCookieAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Cookies.TryGetValue("AclCookie", out var cookieValue))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            // Tạo identity và principal cho anonymous user có ACL
            var claims = new List<Claim>
            {
                new Claim("acl_cookie", cookieValue),
                new Claim(ClaimTypes.Role, "AnonymousWithAcl")
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
