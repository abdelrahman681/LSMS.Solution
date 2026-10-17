using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class AuditLogConfig : IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            builder.ToTable("AuditLogs");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.UserId)
                .IsRequired();

            builder.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.OldValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.NewValues)
                .HasColumnType("nvarchar(max)");

            builder.Property(a => a.CreatedAt).IsRequired();

            builder.Property(a => a.IpAddress)
                .HasMaxLength(45);

            builder.HasIndex(a => a.CreatedAt);
            builder.HasIndex(a => new { a.EntityName, a.EntityId });
        }
    }
}
