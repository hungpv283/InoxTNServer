using InoxServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CategoryId)
                .HasColumnName("category_id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Slug)
                .HasColumnName("slug")
                .HasMaxLength(220)
                .IsRequired();

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasColumnName("description")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.Price)
                .HasColumnName("price")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(x => x.SalePrice)
                .HasColumnName("sale_price")
                .HasColumnType("decimal(12,2)");

            builder.Property(x => x.StockQty)
                .HasColumnName("stock_qty")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.Sku)
                .HasColumnName("sku")
                .HasMaxLength(80)
                .IsRequired();

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.Property(x => x.Material)
                .HasColumnName("material")
                .HasMaxLength(100);

            builder.Property(x => x.Dimensions)
                .HasColumnName("dimensions")
                .HasMaxLength(100);

            builder.Property(x => x.WeightKg)
                .HasColumnName("weight_kg")
                .HasColumnType("decimal(6,2)");

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.IsFeatured)
                .HasColumnName("is_featured")
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(x => x.AvgRating)
                .HasColumnName("avg_rating")
                .HasColumnType("decimal(3,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.ReviewCount)
                .HasColumnName("review_count")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.ViewCount)
                .HasColumnName("view_count")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.CategoryId);
            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.IsFeatured);

            builder.HasOne(x => x.Category)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
