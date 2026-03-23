using InoxServer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InoxServer.Domain.Enums;

namespace InoxServer.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(x => x.OrderNumber)
                .HasColumnName("order_number")
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(x => x.OrderNumber)
                .IsUnique();

            builder.Property(x => x.Subtotal)
                .HasColumnName("subtotal")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(x => x.ShippingFee)
                .HasColumnName("shipping_fee")
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.DiscountAmount)
                .HasColumnName("discount_amount")
                .HasColumnType("decimal(10,2)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(x => x.TotalAmount)
                .HasColumnName("total_amount")
                .HasColumnType("decimal(12,2)")
                .IsRequired();

            builder.Property(x => x.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasDefaultValue(OrderStatus.Pending)
                .IsRequired();

            builder.Property(x => x.ShippingName)
                .HasColumnName("shipping_name")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.ShippingPhone)
                .HasColumnName("shipping_phone")
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.ShippingAddress)
                .HasColumnName("shipping_address")
                .HasColumnType("nvarchar(max)")
                .IsRequired();

            builder.Property(x => x.ShippingProvince)
                .HasColumnName("shipping_province")
                .HasMaxLength(100);

            builder.Property(x => x.Note)
                .HasColumnName("note")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.CancelledReason)
                .HasColumnName("cancelled_reason")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.CreatedAt);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
