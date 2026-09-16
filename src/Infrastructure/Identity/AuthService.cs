using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Application.Interfaces; using CRN.ProductApi.Infrastructure.Data; using Microsoft.EntityFrameworkCore;
namespace CRN.ProductApi.Infrastructure.Identity;
public class AuthService(ApplicationDbContext db,IJwtService jwt) : IAuthService
{ public async Task<AuthResponse> LoginAsync(LoginRequest request,CancellationToken ct){var user=await db.Users.SingleOrDefaultAsync(x=>x.Username==request.Username,ct);if(user is null||!PasswordService.Verify(request.Password,user.PasswordHash))throw new UnauthorizedAccessException("Invalid username or password.");return await jwt.CreateAndPersistTokensAsync(user,ct);} }
