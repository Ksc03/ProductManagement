using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Infrastructure.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable("Item");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity)
               .IsRequired();
    }
}