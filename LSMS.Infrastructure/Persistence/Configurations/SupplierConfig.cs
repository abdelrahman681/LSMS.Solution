using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class SupplierConfig : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.ToTable("Suppliers");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(s => s.Phone)
                .HasMaxLength(30);

            builder.Property(s => s.Email)
                .HasMaxLength(256);

            builder.Property(s => s.Address)
                .HasMaxLength(500);

            builder.Property(s => s.OpeningBalance)
                .HasPrecision(18, 2);

            builder.Property(s => s.IsActive)
                .IsRequired();

            builder.HasMany(s => s.Purchases)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.Payments)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
