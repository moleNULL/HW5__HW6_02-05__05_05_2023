using System.Data;
using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using Microsoft.Data.SqlClient;

namespace HW_ADONET__EFCore.DatabaseHelpers.AdoNetHelpers
{
    internal class AdoNetDatabaseManager : IDatabaseManager
    {
        private const string ORDER_INFO_SQL_EXPRESSION = 
                    @"SELECT o.ord_id, o.ord_datetime, a.an_price, a.an_name, g.gr_name FROM dbo.Orders o
                    INNER JOIN dbo.Analysis a ON a.an_id = o.ord_an
                    INNER JOIN dbo.Groups g ON g.gr_id = a.an_group
                    WHERE Year(o.ord_datetime) = (SELECT Year(MAX(ord_datetime)) FROM dbo.Orders)";

        private readonly string _connectionString;

        public AdoNetDatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<OrderInfo>? GetLastYearOrdersInfo()
        {
            return GetLastYearOrdersViaSqlDataReader();
        }

        public int CreateOrder(OrderEntity order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"INSERT INTO dbo.Orders(ord_datetime, ord_an) VALUES(@ord_datetime, @ord_an);
                                        SELECT SCOPE_IDENTITY()";

                command.Parameters.Add(new SqlParameter("@ord_datetime", order.OrderDateTime));
                command.Parameters.Add(new SqlParameter("@ord_an", order.AnalysisId));

                int id = (int)(decimal)command.ExecuteScalar();
                return id;
            }
        }

        public int UpdateOrder(OrderEntity order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"UPDATE dbo.Orders
                                        SET ord_datetime = @ord_datetime, ord_an = @ord_an
                                        WHERE ord_id = @ord_id";
                command.Parameters.Add(new SqlParameter("@ord_datetime", order.OrderDateTime));
                command.Parameters.Add(new SqlParameter("@ord_an", order.AnalysisId));
                command.Parameters.Add(new SqlParameter("@ord_id", order.Id));

                int count = command.ExecuteNonQuery();
                return count;
            }
        }

        public int DeleteOrder(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @"DELETE FROM dbo.Orders WHERE ord_id = @ord_id";
                command.Parameters.Add(new SqlParameter("@ord_id", id));

                int count = command.ExecuteNonQuery();
                return count;
            }
        }

        public OrderEntity? GetOrderById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM dbo.Orders WHERE ord_id = @ord_id";
                command.Parameters.Add(new SqlParameter("@ord_id", id));

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    var order = new OrderEntity
                    {
                        Id = reader.GetInt32("ord_id"),
                        OrderDateTime = reader.GetDateTime("ord_datetime"),
                        AnalysisId = reader.GetInt32("ord_an")
                    };

                    return order;
                }
            }
        }

        public OrderEntity? GetLastOrder()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT TOP(1) * FROM dbo.Orders ORDER BY ord_id DESC";

                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    var order = new OrderEntity
                    {
                        Id = reader.GetInt32("ord_id"),
                        OrderDateTime = reader.GetDateTime("ord_datetime"),
                        AnalysisId = reader.GetInt32("ord_an")
                    };
                    return order;
                }
            }
        }


        public IEnumerable<OrderInfo>? GetLastYearOrdersViaSqlDataAdapter()
        {
            var ordersInfo = new List<OrderInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var adapter = new SqlDataAdapter(ORDER_INFO_SQL_EXPRESSION, connection))
                {
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);

                    foreach (DataTable table in dataSet.Tables)
                    {
                        if (table.Rows.Count == 0)
                        {
                            return null;
                        }

                        foreach (DataRow row in table.Rows)
                        {
                            var orderInfo = new OrderInfo
                            {
                                Id = (int)row.ItemArray[0]!,
                                OrderDateTime = (DateTime)row.ItemArray[1]!,
                                Price = (decimal)row.ItemArray[2]!,
                                Name = (string)row.ItemArray[3]!,
                                GroupName = (string)row.ItemArray[4]!
                            };

                            ordersInfo.Add(orderInfo);
                        }
                    }

                    return ordersInfo;
                }
            }
        }

        private IEnumerable<OrderInfo>? GetLastYearOrdersViaSqlDataReader()
        {
            var ordersInfo = new List<OrderInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = ORDER_INFO_SQL_EXPRESSION;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var orderInfo = new OrderInfo
                            {
                                Id = reader.GetInt32("ord_id"),
                                OrderDateTime = reader.GetDateTime("ord_datetime"),
                                Price = reader.GetDecimal("an_price"),
                                Name = reader.GetString("an_name"),
                                GroupName = reader.GetString("gr_name")
                            };

                            ordersInfo.Add(orderInfo);
                        }

                        return ordersInfo;
                    }

                    return null;
                }
            }
        }
    }
}
