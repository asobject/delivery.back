
using StackExchange.Redis;
using System.Threading.Tasks;
using YarpApiGateaway.Interfaces;

namespace YarpApiGateaway.Services;

public class RedisRefreshTokenStore(IConnectionMultiplexer redis) : IRefreshTokenStore
{
    private readonly IDatabase _redis = redis.GetDatabase();

   
    public async Task<bool> TokenExistsAsync(string userId, string jti , string refreshToken,string schemePrefix)
    {
        var storedToken = await _redis.StringGetAsync($"{schemePrefix}_REFRESH_TOKENS:{userId}:{jti}");
        return storedToken == refreshToken;
    }
}