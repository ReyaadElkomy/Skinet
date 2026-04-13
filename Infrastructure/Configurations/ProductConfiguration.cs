using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasIndex(x=>x.Name).IsUnique();
        
        builder.Property(x=>x.Price)
                .HasColumnType("decimal(18,2)");
        
        builder.Property(x=>x.Name) 
                .IsRequired()
                .HasMaxLength(200);

        builder.Property(x=>x.Description)
                .HasMaxLength(2000);

        builder.Property(x=>x.Brand)
                .HasMaxLength(150);

        builder.Property(x=>x.Type)
                .HasMaxLength(150);
    }
}
