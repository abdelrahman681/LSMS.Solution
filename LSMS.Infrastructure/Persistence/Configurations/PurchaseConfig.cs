using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class PurchaseConfig : IEntityTypeConfiguration<Purchase>
    {
        public void Configure(EntityTypeBuilder<Purchase> builder)
        {
            builder.ToTable("Purchases");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.PurchaseDate)
                .IsRequired();

            builder.Property(p => p.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.PaidAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(p => p.Supplier)
                .WithMany(s => s.Purchases)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Items)
                .WithOne(i => i.Purchase)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.PurchaseDate);
        }
    }
}
