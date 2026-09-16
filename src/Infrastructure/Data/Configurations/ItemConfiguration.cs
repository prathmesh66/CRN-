using CRN.ProductApi.Domain.Entities; using Microsoft.EntityFrameworkCore; using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace CRN.ProductApi.Infrastructure.Data.Configurations;
public class ItemConfiguration : IEntityTypeConfiguration<Item>{public void Configure(EntityTypeBuilder<Item>b){b.ToTable("Item");b.HasKey(x=>x.Id);b.Property(x=>x.Quantity).IsRequired();b.HasIndex(x=>x.ProductId);}}
