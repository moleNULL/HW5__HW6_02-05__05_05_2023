namespace HW5_ADONET.DatabaseHelpers
{
    public class OrderInfo
    {
        public int Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; } = null!;
        public string GroupName { get; set; } = null!;
    }
}
