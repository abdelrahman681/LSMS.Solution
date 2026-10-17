using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class SaleConfig : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.ToTable("Sales");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.InvoiceNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(s => s.InvoiceNumber)
                .IsUnique();

            builder.Property(s => s.SaleDate)
                .IsRequired();

            builder.Property(s => s.CashierId)
                .IsRequired();

            builder.Property(s => s.Subtotal).HasPrecision(18, 2);
            builder.Property(s => s.DiscountValue).HasPrecision(18, 2);
            builder.Property(s => s.DiscountAmount).HasPrecision(18, 2);
            builder.Property(s => s.TotalAmount).HasPrecision(18, 2);

            builder.Property(s => s.DiscountType)
                .HasConversion<int>()
                .IsRequired();

            builder.HasMany(s => s.Items)
                .WithOne(i => i.Sale)
                .HasForeignKey(i => i.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.SaleDate);
        }
    }
}
