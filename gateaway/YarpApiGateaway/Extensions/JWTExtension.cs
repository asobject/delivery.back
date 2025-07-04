
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using YarpApiGateaway.Interfaces;
using Microsoft.Extensions.Options;

namespace YarpApiGateaway.Extensions;

internal static class JWTExtension
{
    internal static void ConfigureAuthentication(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer("UserAuth", options => ConfigureJwtOptions(options, configuration, validateLifetime: false, "USER"))
.AddJwtBearer("User", options => ConfigureJwtOptions(options, configuration, validateLifetime: true, "USER"))
.AddJwtBearer("AdminAuth", options => ConfigureJwtOptions(options, configuration, validateLifetime: false, "ADMIN"))
.AddJwtBearer("Admin", options => ConfigureJwtOptions(options, configuration, validateLifetime: true, "ADMIN"));

        services.AddAuthorizationBuilder()
            .AddPolicy("UserAuthPolicy", policy =>
            {
                policy.AuthenticationSchemes.Add("UserAuth");
                policy.RequireAuthenticatedUser();
            })
            .AddPolicy("UserPolicy", policy =>
            {
                policy.AuthenticationSchemes.Add("User");
                policy.RequireAuthenticatedUser();
            })
             .AddPolicy("AdminAuthPolicy", policy =>
             {
                 policy.AuthenticationSchemes.Add("AdminAuth");
                 policy.RequireAuthenticatedUser();
             })
              .AddPolicy("AdminPolicy", policy =>
              {
                  policy.AuthenticationSchemes.Add("Admin");
                  policy.RequireAuthenticatedUser();
              })
              .AddPolicy("SuperAdminPolicy", policy =>
              {
                  policy.AuthenticationSchemes.Add("Admin");
                  policy.RequireAuthenticatedUser();
                  policy.RequireRole("SuperAdmin");
              });
    }
    private static void ConfigureJwtOptions(JwtBearerOptions options, IConfiguration configuration, bool validateLifetime, string schemePrefix)
    {
        // Общие настройки для всех схем
        options.TokenValidationParameters = CreateTokenValidationParameters(configuration, validateLifetime, schemePrefix);
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;

        // Настройка событий с учетом префикса схемы
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context => HandleAuthenticationFailed(context, schemePrefix),
            OnTokenValidated = async context => await HandleTokenValidatedAsync(context, schemePrefix)
        };
    }
    private static TokenValidationParameters CreateTokenValidationParameters(IConfiguration configuration, bool validateLifetime, string schemePrefix)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT_AUDIENCE"],
            ValidIssuer = configuration["JWT_ISSUER"],
            ValidateLifetime = validateLifetime,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[$"JWT_{schemePrefix}_SECRET"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    }

    private static Task HandleAuthenticationFailed(AuthenticationFailedContext context, string schemePrefix)
    {
        var logMessage = string.IsNullOrEmpty(schemePrefix)
            ? $"Authentication failed: {context.Exception.Message}"
            : $"[{schemePrefix}] Authentication failed: {context.Exception.Message}";

        Console.WriteLine(logMessage);
        return Task.CompletedTask;
    }

    private async static Task HandleTokenValidatedAsync(TokenValidatedContext context, string schemePrefix)
    {
        try
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var token = authHeader.ToString().Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase);
                var refreshToken = context.Request.Cookies[$"{schemePrefix}_REFRESH_TOKEN"];
                if (string.IsNullOrEmpty(refreshToken))
                    throw new InvalidTokenException("Refresh Token is missing.");
                var handler = new JwtSecurityTokenHandler();

                var jwt = handler.ReadJwtToken(token);
                var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value ?? throw new InvalidTokenException("invalid token");
                var jti = jwt.Claims.FirstOrDefault(c => c.Type == "jti")?.Value ?? throw new InvalidTokenException("invalid token");
                var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? throw new InvalidTokenException("invalid token");

                var tokenStore = context.HttpContext.RequestServices.GetService<IRefreshTokenStore>();

                bool isValid = await tokenStore.TokenExistsAsync(sub, jti, refreshToken, schemePrefix);
                if (!isValid)
                    throw new InvalidTokenException("invalid token");

                context.HttpContext.Request.Headers.Append("X-User-Sub", sub);
                context.HttpContext.Request.Headers.Append("X-User-Email", email);

            }
        }
        catch (Exception ex)
        {
            context.Fail(ex);
        }
    }
}
