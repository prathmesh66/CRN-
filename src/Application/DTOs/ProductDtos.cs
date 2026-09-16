namespace CRN.ProductApi.Application.DTOs;
public record ItemDto(int Id, int Quantity);
public record ProductDto(int Id, string ProductName, string CreatedBy, DateTime CreatedOn, string? ModifiedBy, DateTime? ModifiedOn, IReadOnlyCollection<ItemDto> Items);
public record CreateProductDto(string ProductName, List<int> Quantities);
public record UpdateProductDto(string ProductName, List<int> Quantities);
public record PagedResult<T>(IReadOnlyCollection<T> Items, int PageNumber, int PageSize, int TotalCount, int TotalPages);
public record LoginRequest(string Username, string Password);
public record RefreshRequest(string AccessToken, string RefreshToken);
public record AuthResponse(string AccessToken, string RefreshToken, DateTime AccessTokenExpiresOnUtc, DateTime RefreshTokenExpiresOnUtc);
