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
    public class InventoryLogConfiguration : IEntityTypeConfiguration<InventoryLog>
    {
        public void Configure(EntityTypeBuilder<InventoryLog> builder)
        {
            builder.ToTable("inventory_logs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.ProductId)
                .HasColumnName("product_id")
                .IsRequired();

            builder.Property(x => x.AdminId)
                .HasColumnName("admin_id")
                .IsRequired();

            builder.Property(x => x.Type)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.QuantityChange)
                .HasColumnName("quantity_change")
                .IsRequired();

            builder.Property(x => x.StockBefore)
                .HasColumnName("stock_before")
                .IsRequired();

            builder.Property(x => x.StockAfter)
                .HasColumnName("stock_after")
                .IsRequired();

            builder.Property(x => x.ReferenceNo)
                .HasColumnName("reference_no")
                .HasMaxLength(100);

            builder.Property(x => x.SupplierName)
                .HasColumnName("supplier_name")
                .HasMaxLength(200);

            builder.Property(x => x.UnitCost)
                .HasColumnName("unit_cost")
                .HasColumnType("decimal(12,2)");

            builder.Property(x => x.Note)
                .HasColumnName("note")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.ProductId);
            builder.HasIndex(x => x.AdminId);
            builder.HasIndex(x => x.Type);
            builder.HasIndex(x => x.CreatedAt);

            builder.HasOne(x => x.Product)
                .WithMany(x => x.InventoryLogs)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Admin)
                .WithMany(x => x.InventoryLogs)
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
