using CRN.ProductApi.Application.DTOs;
using CRN.ProductApi.Domain.Entities;
namespace CRN.ProductApi.Application.Interfaces;
public interface IJwtService
{
 (string Token, DateTime ExpiresOnUtc) CreateAccessToken(UserAccount user);
 Task<AuthResponse> CreateAndPersistTokensAsync(UserAccount user, CancellationToken ct);
 Task<AuthResponse> RefreshAsync(string accessToken,string refreshToken,CancellationToken ct);
}
