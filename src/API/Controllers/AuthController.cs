using Asp.Versioning; using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Application.Interfaces; using Microsoft.AspNetCore.Authorization; using Microsoft.AspNetCore.Mvc;
namespace CRN.ProductApi.Controllers;
[ApiController][AllowAnonymous][ApiVersion("1.0")][Route("api/v{version:apiVersion}/auth")]
public class AuthController(IAuthService auth, IJwtService jwt) : ControllerBase
{ [HttpPost("login")] public async Task<ActionResult<AuthResponse>> Login(LoginRequest request,CancellationToken ct)=>Ok(await auth.LoginAsync(request,ct)); [HttpPost("refresh")] public async Task<ActionResult<AuthResponse>> Refresh(RefreshRequest request,CancellationToken ct)=>Ok(await jwt.RefreshAsync(request.AccessToken,request.RefreshToken,ct)); }
