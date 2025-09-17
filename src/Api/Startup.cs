// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // accepts any access token issued by identity server
            services.AddAuthentication("MultiScheme")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                })
                .AddScheme<AuthenticationSchemeOptions, AclCookieAuthenticationHandler>("AclCookie", options => { })
                .AddPolicyScheme("MultiScheme", "JWT or ACL", options =>
                {
                    options.ForwardDefaultSelector = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            return "Bearer"; // nếu có Authorization: Bearer
                        }

                        if (context.Request.Cookies.ContainsKey("AclCookie"))
                        {
                            return "AclCookie"; // nếu có cookie
                        }

                        return "Bearer"; // fallback
                    };
                })
                ;
            
            // adds an authorization policy to make sure the token is for scope 'api1'
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1");
                });

                options.AddPolicy("AclSession", policy =>
                {
                    //policy.RequireAuthenticatedUser();
                    policy.RequireClaim("acl", "s123:read");
                });
            });

            services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();
            services.AddHttpContextAccessor();

        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });
        }
    }
}
