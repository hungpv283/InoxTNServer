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
    public class OrderStatusLogConfiguration : IEntityTypeConfiguration<OrderStatusLog>
    {
        public void Configure(EntityTypeBuilder<OrderStatusLog> builder)
        {
            builder.ToTable("order_status_logs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .IsRequired();

            builder.Property(x => x.AdminId)
                .HasColumnName("admin_id");

            builder.Property(x => x.StatusFrom)
                .HasColumnName("status_from")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.StatusTo)
                .HasColumnName("status_to")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Note)
                .HasColumnName("note")
                .HasColumnType("nvarchar(max)");

            builder.Property(x => x.ChangedAt)
                .HasColumnName("changed_at")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            builder.HasIndex(x => x.OrderId);
            builder.HasIndex(x => x.ChangedAt);

            builder.HasOne(x => x.Order)
                .WithMany(x => x.OrderStatusLogs)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Admin)
                .WithMany(x => x.OrderStatusLogs)
                .HasForeignKey(x => x.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
