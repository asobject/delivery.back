using System.Threading.Tasks;

namespace YarpApiGateaway.Interfaces;

public interface IRefreshTokenStore
{
    Task<bool> TokenExistsAsync(string userId, string jti,string refreshToken, string schemePrefix);
}