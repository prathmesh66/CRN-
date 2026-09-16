using System.Text.Json; using CRN.ProductApi.Domain.Exceptions; using FluentValidation; using Microsoft.IdentityModel.Tokens;
namespace CRN.ProductApi.Middleware;
public class ExceptionHandlingMiddleware(RequestDelegate next,ILogger<ExceptionHandlingMiddleware> logger)
{
 public async Task InvokeAsync(HttpContext context)
 {
  try { await next(context); }
  catch (Exception ex)
  {
   logger.LogError(ex, "Unhandled exception for {Path}", context.Request.Path);
   context.Response.ContentType = "application/json";
   var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
   if (ex is ValidationException fvEx)
   {
    context.Response.StatusCode = 400;
    var errors = fvEx.Errors.Select(e => new { field = char.ToLowerInvariant(e.PropertyName[0]) + e.PropertyName.Substring(1), message = e.ErrorMessage }).ToList();
    var valBody = new { statusCode = 400, message = "Validation failed", errors };
    await context.Response.WriteAsync(JsonSerializer.Serialize(valBody, jsonOptions));
    return;
   }
   context.Response.StatusCode = ex switch { NotFoundException => 404, UnauthorizedAccessException => 401, SecurityTokenException => 401, ArgumentException => 400, _ => 500 };
   var body = new { statusCode = context.Response.StatusCode, message = context.Response.StatusCode == 500 ? "An unexpected error occurred." : ex.Message };
   await context.Response.WriteAsync(JsonSerializer.Serialize(body, jsonOptions));
  }
 }
}
