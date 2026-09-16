using CRN.ProductApi.Application.Interfaces;
namespace CRN.ProductApi.Infrastructure.Data;
public sealed class UnitOfWork(ApplicationDbContext db) : IUnitOfWork { public Task<int> SaveChangesAsync(CancellationToken ct=default)=>db.SaveChangesAsync(ct); }
