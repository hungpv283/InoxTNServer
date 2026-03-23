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
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.ToTable("coupons");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Code)
                .HasColumnName("code")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(x => x.Value)
                .HasColumnName("value")
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.MinOrderValue)
                .HasColumnName("min_order_value")
                .HasColumnType("decimal(12,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.MaxDiscount)
                .HasColumnName("max_discount")
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.UsageLimit)
                .HasColumnName("usage_limit");

            builder.Property(x => x.UsedCount)
                .HasColumnName("used_count")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(x => x.StartsAt)
                .HasColumnName("starts_at");

            builder.Property(x => x.ExpiresAt)
                .HasColumnName("expires_at");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.IsActive);
            builder.HasIndex(x => x.ExpiresAt);
        }
    }
}
