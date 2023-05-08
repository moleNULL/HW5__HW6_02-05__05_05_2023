using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.HasKey(o => o.Id).HasName("PK__Orders__DC39D7DF12B77EF3");

            builder.Property(o => o.Id).HasColumnName("ord_id");
            builder.Property(o => o.AnalysisId).HasColumnName("ord_an");
            builder.Property(o => o.OrderDateTime)
                    .HasColumnType("datetime")
                    .HasColumnName("ord_datetime");

            builder.HasOne(o => o.Analysis).WithMany(a => a.Orders)
                    .HasForeignKey(o => o.AnalysisId)
                    .HasConstraintName("FK_Orders_To_Analysis");
        }
    }
}
