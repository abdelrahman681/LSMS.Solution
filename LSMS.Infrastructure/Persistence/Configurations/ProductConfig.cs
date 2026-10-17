using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Barcode)
                .HasMaxLength(100);

            builder.Property(p => p.SKU)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(p => p.SKU)
                .IsUnique();

            builder.HasIndex(p => p.Barcode)
                .IsUnique()
                .HasFilter("[Barcode] IS NOT NULL");

            builder.Property(p => p.UnitCost)
                .HasPrecision(18, 2);

            builder.Property(p => p.SellingPrice)
                .HasPrecision(18, 2);

            builder.Property(p => p.StockQuantity)
                .IsRequired();

            builder.Property(p => p.ReorderLevel)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
