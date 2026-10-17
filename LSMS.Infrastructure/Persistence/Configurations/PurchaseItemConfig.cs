using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class PurchaseItemConfig : IEntityTypeConfiguration<PurchaseItem>
    {
        public void Configure(EntityTypeBuilder<PurchaseItem> builder)
        {
            builder.ToTable("PurchaseItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.UnitCost)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(i => i.LineTotal)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.HasOne(i => i.Purchase)
                .WithMany(p => p.Items)
                .HasForeignKey(i => i.PurchaseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(i => i.Product)
                .WithMany()
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
