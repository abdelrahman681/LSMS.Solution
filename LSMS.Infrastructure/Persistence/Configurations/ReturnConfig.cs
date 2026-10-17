using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class ReturnConfig : IEntityTypeConfiguration<Return>
    {
        public void Configure(EntityTypeBuilder<Return> builder)
        {
            builder.ToTable("Returns");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.ReturnDate).IsRequired();

            builder.Property(r => r.CashierId).IsRequired();

            builder.Property(r => r.TotalAmount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(r => r.Notes)
                .HasMaxLength(1000);

            builder.HasOne(r => r.Sale)
                .WithOne()
                .HasForeignKey<Return>(r => r.SaleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(r => r.ReturnDate);
        }
    }
}
