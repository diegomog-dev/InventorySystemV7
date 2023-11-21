using InventorySystemV7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventorySystemV7.DataAccess.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.SerialNumber).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Cost).IsRequired();
            builder.Property(x => x.CategoryId).IsRequired();
            builder.Property(x => x.BrandId).IsRequired();
            builder.Property(x => x.UrlImage).IsRequired(false);
            builder.Property(x => x.FatherId).IsRequired(false);

            /* Table relationships */
            builder.HasOne(x => x.Category).WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Brand).WithMany()
                .HasForeignKey(x => x.BrandId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Father).WithMany()
                .HasForeignKey(x => x.FatherId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
