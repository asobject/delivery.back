
using Application.Interfaces.Services;
using BuildingBlocks.Exceptions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Infrastructure.Services;

public class RedisRefreshTokenStore(IConnectionMultiplexer redis, IConfiguration config) : IRefreshTokenStore
{
    private readonly IDatabase _redis = redis.GetDatabase();
    private readonly TimeSpan _tokenLifetime = TimeSpan.FromDays(config.GetValue<int>("JWT_USER_REFRESH_TOKEN_VALIDITY"));

    public async Task StoreTokenAsync(string userId, string jti, string refreshToken)
    {
        var key = $"USER_REFRESH_TOKENS:{userId}:{jti}";
        await _redis.StringSetAsync(key, refreshToken, _tokenLifetime);
    }

    public async Task<bool> UpdateTokenAsync(
       string userId,
       string oldJti,
       string newJti,
       string oldRefreshToken,
       string newRefreshToken)
    {
        var storedToken = await _redis.StringGetAsync($"USER_REFRESH_TOKENS:{userId}:{oldJti}");
        if (storedToken != oldRefreshToken)
        {
            throw new InvalidTokenException("Invalid refresh token");
        }

        await _redis.KeyDeleteAsync($"USER_REFRESH_TOKENS:{userId}:{oldJti}");

        await StoreTokenAsync(userId, newJti, newRefreshToken);
        return true;
    }
    public async Task RevokeAllTokensAsync(string userId)
    {
        var endpoints = redis.GetEndPoints();
        if (endpoints.Length == 0)
            throw new NotFoundException("no redis endpoints found");

        var server = redis.GetServer(endpoints.First());
        var pattern = $"USER_REFRESH_TOKENS:{userId}:*";
        var keys = server.Keys(pattern: pattern);

        if (keys.Any())
        {
            await _redis.KeyDeleteAsync([.. keys.Select(k => (RedisKey)k)]);
        }
    }
    public async Task RevokeTokenAsync(string userId, string jti)
    {
        if (!await _redis.KeyDeleteAsync($"USER_REFRESH_TOKENS:{userId}:{jti}"))
        {
            throw new InvalidTokenException("Token not found");
        }
    }
    public async Task<bool> TokenExistsAsync(string userId, string jti)
    {
        return await _redis.KeyExistsAsync($"USER_REFRESH_TOKENS:{userId}:{jti}");
    }
    public async Task<string?> GetTokenAsync(string userId, string jti)
    {
        var response = await _redis.StringGetAsync($"USER_REFRESH_TOKENS:{userId}:{jti}");
        if (response == RedisValue.Null)
            throw new NotFoundException("Refresh token not found");
        return response;
    }
}