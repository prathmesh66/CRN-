using CRN.ProductApi.Domain.Entities;
namespace CRN.ProductApi.Application.Interfaces;
public interface IProductRepository
{
 Task<Product?> GetByIdAsync(int id, CancellationToken ct);
 Task<Product?> GetTrackedByIdAsync(int id, CancellationToken ct);
 void ReplaceItems(Product product);
 Task<(IReadOnlyList<Product> Items,int TotalCount)> GetPagedAsync(int page,int pageSize,CancellationToken ct);
 Task AddAsync(Product product,CancellationToken ct);
 void Update(Product product);
 void Remove(Product product);
}
