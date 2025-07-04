﻿
using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisRefreshTokenStore(IConnectionMultiplexer redis, IConfiguration config) : IRefreshTokenStore
{
    private readonly IDatabase _redis = redis.GetDatabase();
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromDays(config.GetValue<int>("JWT_ADMIN_REFRESH_TOKEN_VALIDITY"));

    public async Task StoreTokenAsync(string userId, string jti, string refreshToken)
    {
        var key = $"ADMIN_REFRESH_TOKENS:{userId}:{jti}";
        await _redis.StringSetAsync(key, refreshToken, _tokenLifetime);
    }

    public async Task<bool> UpdateTokenAsync(
       string userId,
       string oldJti,
       string newJti,
       string oldRefreshToken,
       string newRefreshToken)
    {
        var storedToken = await _redis.StringGetAsync($"ADMIN_REFRESH_TOKENS:{userId}:{oldJti}");
        if (storedToken != oldRefreshToken)
        {
            throw new InvalidTokenException("Invalid refresh token");
        }

        await _redis.KeyDeleteAsync($"ADMIN_REFRESH_TOKENS:{userId}:{oldJti}");

        await StoreTokenAsync(userId, newJti, newRefreshToken);
        return true;
    }
    public async Task RevokeTokenAsync(string userId, string jti)
    {
        if (!await _redis.KeyDeleteAsync($"ADMIN_REFRESH_TOKENS:{userId}:{jti}"))
        {
            throw new InvalidTokenException("Token not found");
        }
    }
    public async Task<bool> TokenExistsAsync(string userId, string jti)
    {
        return await _redis.KeyExistsAsync($"admin_refresh_tokens:{userId}:{jti}");
    }
    public async Task<string> GetTokenAsync(string userId, string jti)
    {
        var response = await _redis.StringGetAsync($"ADMIN_REFRESH_TOKENS:{userId}:{jti}");
        if (response == RedisValue.Null)
            throw new NotFoundException("Refresh token not found");
        return response;
    }
}