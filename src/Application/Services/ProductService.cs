using AutoMapper; using CRN.ProductApi.Application.DTOs; using CRN.ProductApi.Application.Interfaces; using CRN.ProductApi.Domain.Entities; using CRN.ProductApi.Domain.Exceptions;
namespace CRN.ProductApi.Application.Services;
public class ProductService(IProductRepository repo,IUnitOfWork uow,IMapper mapper) : IProductService
{
 public async Task<PagedResult<ProductDto>> GetPagedAsync(int page,int pageSize,CancellationToken ct){page=Math.Max(1,page);pageSize=Math.Clamp(pageSize,1,100);var(r,total)=await repo.GetPagedAsync(page,pageSize,ct);var totalPages=pageSize>0?(int)Math.Ceiling(total/(double)pageSize):0;return new PagedResult<ProductDto>(r.Select(mapper.Map<ProductDto>).ToList(),page,pageSize,total,totalPages);}
 public async Task<ProductDto> GetByIdAsync(int id,CancellationToken ct){var p=await repo.GetByIdAsync(id,ct)??throw new NotFoundException($"Product {id} was not found.");return mapper.Map<ProductDto>(p);}
 public async Task<IReadOnlyCollection<ItemDto>> GetItemsByProductIdAsync(int id,CancellationToken ct){var p=await GetByIdAsync(id,ct);return p.Items;}
 public async Task<ProductDto> CreateAsync(CreateProductDto dto,string user,CancellationToken ct){var p=new Product{ProductName=dto.ProductName.Trim(),CreatedBy=user,CreatedOn=DateTime.UtcNow};p.Items=dto.Quantities.Select(q=>new Item{Quantity=q}).ToList();await repo.AddAsync(p,ct);await uow.SaveChangesAsync(ct);return mapper.Map<ProductDto>(p);}
 public async Task<ProductDto> UpdateAsync(int id,UpdateProductDto dto,string user,CancellationToken ct){var p=await repo.GetTrackedByIdAsync(id,ct)??throw new NotFoundException($"Product {id} was not found.");p.ProductName=dto.ProductName.Trim();p.ModifiedBy=user;p.ModifiedOn=DateTime.UtcNow;repo.ReplaceItems(p);p.Items=dto.Quantities.Select(q=>new Item{ProductId=id,Quantity=q}).ToList();repo.Update(p);await uow.SaveChangesAsync(ct);return await GetByIdAsync(id,ct);}
 public async Task DeleteAsync(int id,CancellationToken ct){var p=await repo.GetByIdAsync(id,ct)??throw new NotFoundException($"Product {id} was not found.");repo.Remove(p);await uow.SaveChangesAsync(ct);}
}
