namespace HW_ADONET__EFCore.DatabaseHelpers.Entities;

public class OrderEntity
{
    public int Id { get; set; }

    public DateTime OrderDateTime { get; set; }

    public int AnalysisId { get; set; }

    public AnalysisEntity Analysis { get; set; } = null!;
}
