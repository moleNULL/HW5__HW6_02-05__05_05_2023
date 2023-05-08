using HW_ADONET__EFCore.DatabaseHelpers.Entities;

namespace HW_ADONET__EFCore.DatabaseHelpers
{
    internal interface IDatabaseManager
    {
        IEnumerable<OrderInfo>? GetLastYearOrdersInfo();
        int CreateOrder(OrderEntity order);
        int UpdateOrder(OrderEntity order);
        int DeleteOrder(int id);
        OrderEntity? GetOrderById(int id);
        OrderEntity? GetLastOrder();
    }
}
