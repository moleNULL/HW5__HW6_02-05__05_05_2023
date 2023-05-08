namespace HW_ADONET__EFCore.DatabaseHelpers.Entities;

public class AnalysisEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Cost { get; set; }

    public decimal Price { get; set; }

    public int GroupId { get; set; }

    public virtual GroupEntity Group { get; set; } = null!;

    public virtual ICollection<OrderEntity> Orders { get; set; } = new List<OrderEntity>();
}
