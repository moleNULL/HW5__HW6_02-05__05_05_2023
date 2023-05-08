namespace HW_ADONET__EFCore.DatabaseHelpers.Entities;

public class GroupEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Temperature { get; set; }

    public virtual ICollection<AnalysisEntity> Analyses { get; set; } = new List<AnalysisEntity>();
}
