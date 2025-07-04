﻿using BuildingBlocks.Exceptions;
using BuildingBlocks.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Services;

public class TokenExtractionService(IHttpContextAccessor httpContextAccessor, IAppConfiguration appConfiguration) : ITokenExtractionService
{
    private static readonly string CookieKey = "ADMIN_REFRESH_TOKEN";
    public string GetAccessTokenFromHeader()
    {
        var context = httpContextAccessor.HttpContext ??
           throw new InvalidOperationException("No active HTTP context.");

        if (!context.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeader))
            throw new InvalidOperationException("Authorization header is missing.");
        var access = authorizationHeader.ToString().Replace("Bearer ", "").Trim();
        return access;
    }

    public string GetRefreshTokenFromCookie()
    {
        var context = httpContextAccessor.HttpContext ??
          throw new InvalidOperationException("No active HTTP context.");

        var refreshToken = context.Request.Cookies[CookieKey];
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidTokenException("Refresh Token is missing.");
        return refreshToken;
    }

    public void RemoveRefreshTokenFromCookie()
    {
        var context = httpContextAccessor.HttpContext ??
        throw new InvalidOperationException("No active HTTP context.");

        context.Response.Cookies.Delete(CookieKey);
    }

    public void SetRefreshTokenInCookie(string refreshToken)
    {
        var context = httpContextAccessor.HttpContext ??
         throw new InvalidOperationException("No active HTTP context.");
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(Convert.ToDouble(appConfiguration.GetValue("JWT_ADMIN_REFRESH_TOKEN_VALIDITY")))
        };
        context.Response.Cookies.Append(CookieKey, refreshToken, cookieOptions);
    }
}
