using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ProductName)
               .IsRequired()
               .HasMaxLength(255);

        builder.Property(x => x.CreatedBy)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.ModifiedBy)
               .HasMaxLength(100);

        builder.Property(x => x.CreatedOn)
               .IsRequired();

        builder.HasMany(x => x.Items)
               .WithOne(x => x.Product)
               .HasForeignKey(x => x.ProductId);
    }
}