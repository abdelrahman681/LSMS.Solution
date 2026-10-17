using LSMS.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LSMS.Infrastructure.Persistence.Configurations
{


    public class ProductImageConfiguration: IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.ImageUrl)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(pi => pi.DisplayOrder)
                .IsRequired();

            builder.Property(pi => pi.IsPrimary)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(pi => new { pi.ProductId, pi.DisplayOrder });

            // A product can have at most one primary image.
            builder.HasIndex(pi => pi.ProductId)
                .IsUnique()
                .HasFilter("[IsPrimary] = 1");
        }
    }

}
