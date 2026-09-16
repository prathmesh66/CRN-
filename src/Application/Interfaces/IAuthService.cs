using CRN.ProductApi.Application.DTOs;
namespace CRN.ProductApi.Application.Interfaces;
public interface IAuthService
{
 Task<AuthResponse> LoginAsync(LoginRequest request,CancellationToken ct);
}
