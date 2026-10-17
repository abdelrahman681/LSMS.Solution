using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class CashMovementConfig : IEntityTypeConfiguration<CashMovement>
    {
        public void Configure(EntityTypeBuilder<CashMovement> builder)
        {
            builder.ToTable("CashMovements");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.MovementType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(m => m.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(m => m.MovementDate).IsRequired();

            builder.Property(m => m.ReferenceType)
                .HasMaxLength(100);

            builder.Property(m => m.Notes)
                .HasMaxLength(1000);

            builder.Property(m => m.UserId)
                .IsRequired();

            builder.HasIndex(m => m.MovementDate);
            builder.HasIndex(m => new { m.ReferenceType, m.ReferenceId });
        }
    }
}
