using CRN.ProductApi.Application.DTOs;
namespace CRN.ProductApi.Application.Interfaces;
public interface IProductService
{
 Task<PagedResult<ProductDto>> GetPagedAsync(int page,int pageSize,CancellationToken ct);
 Task<ProductDto> GetByIdAsync(int id,CancellationToken ct);
 Task<IReadOnlyCollection<ItemDto>> GetItemsByProductIdAsync(int id,CancellationToken ct);
 Task<ProductDto> CreateAsync(CreateProductDto dto,string user,CancellationToken ct);
 Task<ProductDto> UpdateAsync(int id,UpdateProductDto dto,string user,CancellationToken ct);
 Task DeleteAsync(int id,CancellationToken ct);
}
