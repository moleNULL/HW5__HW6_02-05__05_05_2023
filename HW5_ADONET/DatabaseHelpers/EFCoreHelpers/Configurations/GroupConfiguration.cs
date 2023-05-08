using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.HasKey(g => g.Id).HasName("PK__Groups__2BC0F88EBA28B23C");

            builder.Property(g => g.Id).HasColumnName("gr_id");
            builder.Property(g => g.Name)
                .HasMaxLength(50)
                .HasColumnName("gr_name");
            builder.Property(g => g.Temperature)
                .HasColumnType("decimal(4, 1)")
                .HasColumnName("gr_temp");
        }
    }
}
