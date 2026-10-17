using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    internal class StockMovementConfig : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.ToTable("StockMovements");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.MovementType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(m => m.Quantity).IsRequired();
            builder.Property(m => m.StockBefore).IsRequired();
            builder.Property(m => m.StockAfter).IsRequired();
            builder.Property(m => m.MovementDate).IsRequired();

            builder.Property(m => m.ReferenceType)
                .HasMaxLength(100);

            builder.Property(m => m.Notes)
                .HasMaxLength(1000);

            builder.HasOne(m => m.Product)
                .WithMany()
                .HasForeignKey(m => m.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(m => m.MovementDate);
            builder.HasIndex(m => new { m.ReferenceType, m.ReferenceId });
        }
    }
}
