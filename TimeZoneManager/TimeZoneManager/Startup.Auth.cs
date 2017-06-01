using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNet.Security.OpenIdConnect.Extensions;
using AspNet.Security.OpenIdConnect.Primitives;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TimeZoneManager.Data.Models;
using TimeZoneManager.Services.Common;
using TimeZoneManager.Services.Common.Config;

namespace TimeZoneManager
{
    public partial class Startup
    {
        private void ConfigureAuth(
            IApplicationBuilder app, 
            SignInManager<TimeZoneAppUser> signInManager, 
            UserManager<TimeZoneAppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            AuthConfiguration authConfiguration)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authConfiguration.Key));

            app.UseOpenIdConnectServer(options =>
            {
                options.TokenEndpointPath = "/connect/token";
                options.AllowInsecureHttp = true;
                options.AccessTokenHandler = new JwtSecurityTokenHandler();
                options.AccessTokenLifetime = TimeSpan.FromHours(24);
                
                options.Issuer = new Uri(authConfiguration.Issuer);

                options.SigningCredentials.AddKey(signingKey);

                options.Provider.OnValidateTokenRequest = context =>
                {
                    if (!context.Request.IsPasswordGrantType())
                    {
                        context.Reject(
                            error: OpenIdConnectConstants.Errors.UnsupportedGrantType,
                            description: "Only grant_type=password and refresh_token " +
                                         "requests are accepted by this server.");

                        return Task.FromResult(0);
                    }

                    context.Validate();

                    return Task.FromResult(0);
                };

                options.Provider.OnHandleTokenRequest = async context =>
                {
                    if (context.Request.IsPasswordGrantType())
                    {
                        var username = context.Request.Username;
                        var password = context.Request.Password;

                        var user = await userManager.FindByNameAsync(username);
                        if (user == null)
                        {
                            context.Reject(OpenIdConnectConstants.Errors.InvalidGrant, "Invalid user credentials.");
                            return;
                        }

                        var result = await signInManager.CheckPasswordSignInAsync(user, password, false);
                        if (!result.Succeeded)
                        {
                            context.Reject(OpenIdConnectConstants.Errors.InvalidGrant, "Invalid user credentials.");
                            return;
                        }

                        var factory = new UserClaimsPrincipalFactory<TimeZoneAppUser, IdentityRole>(userManager, roleManager, optionsAccessor);
                        var claimsPrincipal = await factory.CreateAsync(user);
                        var identity = (ClaimsIdentity)claimsPrincipal.Identity;

                        identity.AddClaim(OpenIdConnectConstants.Claims.Subject, user.UserName);

                        var roleClaims = identity.Claims.Where(c => c.Type == ClaimTypes.Role).GroupBy(g => g.Value).Select(g => g.First());
                        
                        foreach (Claim roleClaim in roleClaims)
                        {
                            identity.AddClaim(CustomClaimTypes.Permission, roleClaim.Value,
                                OpenIdConnectConstants.Destinations.AccessToken,
                                OpenIdConnectConstants.Destinations.IdentityToken);
                        }

                        identity.AddClaim(CustomClaimTypes.Name, user.DisplayName,
                            OpenIdConnectConstants.Destinations.AccessToken,
                            OpenIdConnectConstants.Destinations.IdentityToken);
                        identity.AddClaim(CustomClaimTypes.UserId, user.Id,
                            OpenIdConnectConstants.Destinations.AccessToken,
                            OpenIdConnectConstants.Destinations.IdentityToken);
                        var ticket = new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), context.Options.AuthenticationScheme);

                        //ticket.SetScopes(OpenIdConnectConstants.Scopes.OfflineAccess);

                        ticket.SetAudiences(authConfiguration.Audience);
                        context.Validate(ticket);
                    }
                };
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = authConfiguration.Issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = authConfiguration.Audience,

                // Validate the token expiry
                ValidateLifetime = true,

                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}
