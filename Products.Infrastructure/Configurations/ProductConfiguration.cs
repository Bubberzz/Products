using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Domain.ValueObjects;

namespace Products.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                productId => productId.Value,
                value => new ProductId(value))
            .IsRequired();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(p => p.Price, price =>
        {
            price.Property(p => p.Value)
                .HasColumnName("Price")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.OwnsOne(p => p.Stock, stock =>
        {
            stock.Property(s => s.Value)
                .HasColumnName("Stock")
                .IsRequired();
        });
        
        // builder.HasKey(p => p.Id);
        //
        // builder.Property(p => p.Id)
        //     .HasConversion(
        //         productId => productId.Value,
        //         value => new ProductId(value))
        //     .IsRequired();
        //
        // builder.Property(p => p.Name)
        //     .IsRequired()
        //     .HasMaxLength(100);
        //
        // builder.Property(p => p.Price).HasConversion(
        //     price => price.Value,
        //     value => new Price(value));
        //
        // builder.Property(p => p.Stock).HasConversion(
        //     stock => stock.Value,
        //     value => new Stock(value));
    }
}