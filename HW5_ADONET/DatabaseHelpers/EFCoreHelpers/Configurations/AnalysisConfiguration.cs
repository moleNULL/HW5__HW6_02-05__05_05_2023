using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers.Configurations
{
    public class AnalysisConfiguration : IEntityTypeConfiguration<AnalysisEntity>
    {
        public void Configure(EntityTypeBuilder<AnalysisEntity> builder)
        {
            builder.HasKey(a => a.Id).HasName("PK__Analysis__831DABF37D557093");

            builder.ToTable("Analysis");

            builder.Property(a => a.Id).HasColumnName("an_id");
            builder.Property(a => a.Cost)
                .HasColumnType("money")
                .HasColumnName("an_cost");
            builder.Property(a => a.GroupId).HasColumnName("an_group");
            builder.Property(a => a.Name)
                .HasMaxLength(100)
                .HasColumnName("an_name");
            builder.Property(a => a.Price)
                .HasColumnType("money")
                .HasColumnName("an_price");

            builder.HasOne(a => a.Group).WithMany(g => g.Analyses)
                .HasForeignKey(a => a.GroupId)
                .HasConstraintName("FK_Analysis_To_Groups");
        }
    }
}
