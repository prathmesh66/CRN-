using CRN.ProductApi.Domain.Entities; using Microsoft.EntityFrameworkCore;
namespace CRN.ProductApi.Infrastructure.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
 public DbSet<Product> Products => Set<Product>(); public DbSet<Item> Items=>Set<Item>(); public DbSet<UserAccount> Users=>Set<UserAccount>(); public DbSet<RefreshToken> RefreshTokens=>Set<RefreshToken>();
 protected override void OnModelCreating(ModelBuilder modelBuilder){base.OnModelCreating(modelBuilder); modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);}
}
