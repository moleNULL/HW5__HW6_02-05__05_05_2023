using System.Data;
using Microsoft.Data.SqlClient;
using HW5_ADONET.DatabaseHelpers.Entities;

namespace HW5_ADONET.DatabaseHelpers
{
    internal class DatabaseManager
    {
        private readonly string _connectionString;

        public DatabaseManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<OrderInfo>? GetOrdersViaSqlDataReader(string sqlExpression)
        {
            var ordersInfo = new List<OrderInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = sqlExpression;

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

        public IEnumerable<OrderInfo>? GetOrdersViaSqlDataAdapter(string sqlExpression)
        {
            var ordersInfo = new List<OrderInfo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var adapter = new SqlDataAdapter(sqlExpression, connection))
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

        public int CreateOrder(OrdersEntity order)
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

        public int UpdateOrder(OrdersEntity order)
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

        public OrdersEntity? GetOrderById(int id)
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

                    var order = new OrdersEntity
                    {
                        Id = reader.GetInt32("ord_id"),
                        OrderDateTime = reader.GetDateTime("ord_datetime"),
                        AnalysisId = reader.GetInt32("ord_an")
                    };

                    return order;
                }
            }
        }

        public OrdersEntity? GetLastOrder()
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

                    var order = new OrdersEntity
                    {
                        Id = reader.GetInt32("ord_id"),
                        OrderDateTime = reader.GetDateTime("ord_datetime"),
                        AnalysisId = reader.GetInt32("ord_an")
                    };
                    return order;
                }
            }
        }
    }
}
