using CRN.ProductApi.Domain.Entities; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CRN.ProductApi.Infrastructure.Data.Configurations;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>{public void Configure(EntityTypeBuilder<RefreshToken>b){b.ToTable("RefreshToken");b.HasKey(x=>x.Id);b.Property(x=>x.TokenHash).HasMaxLength(128).IsRequired();b.HasIndex(x=>x.TokenHash).IsUnique();b.Property(x=>x.CreatedOnUtc).IsRequired();b.Property(x=>x.ExpiresOnUtc).IsRequired();}}
