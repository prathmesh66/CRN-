using CRN.ProductApi.Application.Interfaces; using CRN.ProductApi.Domain.Entities; using Microsoft.EntityFrameworkCore;
namespace CRN.ProductApi.Infrastructure.Data.Repositories;
public class ProductRepository(ApplicationDbContext db) : IProductRepository
{
 public Task<Product?> GetByIdAsync(int id,CancellationToken ct)=>db.Products.AsNoTracking().Include(x=>x.Items).FirstOrDefaultAsync(x=>x.Id==id,ct);
 public Task<Product?> GetTrackedByIdAsync(int id,CancellationToken ct)=>db.Products.Include(x=>x.Items).FirstOrDefaultAsync(x=>x.Id==id,ct);
 public void ReplaceItems(Product product){db.Items.RemoveRange(product.Items);product.Items.Clear();}
 public async Task<(IReadOnlyList<Product> Items,int TotalCount)> GetPagedAsync(int page,int pageSize,CancellationToken ct){var q=db.Products.AsNoTracking().Include(x=>x.Items).OrderBy(x=>x.Id);var total=await q.CountAsync(ct);var items=await q.Skip((page-1)*pageSize).Take(pageSize).ToListAsync(ct);return(items,total);}
 public Task AddAsync(Product product,CancellationToken ct)=>db.Products.AddAsync(product,ct).AsTask(); public void Update(Product product)=>db.Products.Update(product); public void Remove(Product product)=>db.Products.Remove(product);
}
