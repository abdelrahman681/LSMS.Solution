using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{
    public class ExpenseConfig : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(e => e.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(e => e.ExpenseDate).IsRequired();

            builder.Property(e => e.Notes)
                .HasMaxLength(1000);

            builder.Property(e => e.UserId)
                .IsRequired();

            builder.HasIndex(e => e.ExpenseDate);
        }
    }
}
