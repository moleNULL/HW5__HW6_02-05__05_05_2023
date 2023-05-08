using HW_ADONET__EFCore.DatabaseHelpers.EFCoreRepositories;
using HW_ADONET__EFCore.DatabaseHelpers.Entities;

namespace HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers
{
    internal class EFCoreDatabaseManager : IDatabaseManager
    {
        private readonly string _connectionString;
        public EFCoreDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int CreateOrder(OrderEntity order)
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                return order.Id;
            }
        }

        public int DeleteOrder(int id)
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                var order = dbContext.Orders.FirstOrDefault(o => o.Id == id);

                if (order is not null)
                {
                    dbContext.Orders.Remove(order);
                    int count = dbContext.SaveChanges();

                    return count;
                }

                return 0;
            }
        }

        public OrderEntity? GetLastOrder()
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                return dbContext.Orders.OrderByDescending(o => o.Id).FirstOrDefault();
            }
        }

        public OrderEntity? GetOrderById(int id)
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                return dbContext.Orders.FirstOrDefault(o => o.Id == id);
            }
        }

        public IEnumerable<OrderInfo>? GetLastYearOrdersInfo()
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                var orderInfo = (from o in dbContext.Orders
                                 join a in dbContext.Analyses on o.AnalysisId equals a.Id
                                 join g in dbContext.Groups on a.GroupId equals g.Id
                                 where o.OrderDateTime.Year == dbContext.Orders.Max(o => o.OrderDateTime.Year)
                                 select new OrderInfo
                                 {
                                     Id = o.Id,
                                     OrderDateTime = o.OrderDateTime,
                                     Price = a.Price,
                                     Name = a.Name,
                                     GroupName = g.Name
                                 }).ToList();

                return orderInfo.Any() ? orderInfo : null;
            }
        }

        public int UpdateOrder(OrderEntity order)
        {
            using (var dbContext = new ChiHomeworkDbContext(_connectionString))
            {
                var existingOrder = dbContext.Orders.FirstOrDefault(o => o.Id == order.Id);

                if (existingOrder is not null)
                {
                    dbContext.Entry(existingOrder).CurrentValues.SetValues(order);
                    int count = dbContext.SaveChanges();

                    return count;
                }

                return 0;
            }
        }
    }
}
