using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class SupplierPaymentConfig : IEntityTypeConfiguration<SupplierPayment>
    {
        public void Configure(EntityTypeBuilder<SupplierPayment> builder)
        {
            builder.ToTable("SupplierPayments");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.PaymentDate)
                .IsRequired();

            builder.Property(p => p.Notes)
                .HasMaxLength(500);

            builder.HasOne(p => p.Supplier)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Purchase)
                .WithMany()
                .HasForeignKey(p => p.PurchaseId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => p.PaymentDate);
        }
    }
}
