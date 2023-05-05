using Microsoft.Extensions.Configuration;
using HW5_ADONET.DatabaseHelpers;
using HW5_ADONET.DatabaseHelpers.Entities;

namespace HW5_ADONET
{
    internal class Starter
    {
        public static void Run()
        {
            string? connectionString = GetConnectionString();
            if (connectionString is null)
            {
                ColorConsole.WriteLine("Incorrect connection string or missing", ConsoleColor.Red);
                return;
            }

            string separator = new string('-', Console.WindowWidth);

            string orderInfoSqlExpression = @"SELECT o.ord_id, o.ord_datetime, a.an_price, a.an_name, g.gr_name 
                             FROM dbo.Orders o
                             INNER JOIN dbo.Analysis a ON a.an_id = o.ord_an
                             INNER JOIN dbo.Groups g ON g.gr_id = a.an_group
                             WHERE Year(o.ord_datetime) = (SELECT Year(MAX(ord_datetime)) FROM dbo.Orders)";

            var databaseManager = new DatabaseManager(connectionString);

            PrintTask1(separator, orderInfoSqlExpression, databaseManager);
            PrintTask2(separator, orderInfoSqlExpression, databaseManager);
            PrintTask3(separator, databaseManager);
            PrintTask4(separator, databaseManager);
            PrintTask5(separator, databaseManager);
        }

        private static string? GetConnectionString()
        {
            return new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile(path: "appsettings.json", optional: true, reloadOnChange: true)
                        .Build()
                        .GetConnectionString("SQLServerConnection");
        }

        private static void PrintTask1(string separator,string orderInfoSqlExpression, DatabaseManager databaseManager)
        {
            Console.WriteLine(separator);
            ColorConsole.WriteLine("Task_1. Print all orders for the last year using SqlCommand/SqlDataReader:\n",
                ConsoleColor.Green);

            try
            {
                var ordersInfo = databaseManager.GetOrdersViaSqlDataReader(orderInfoSqlExpression);
                PrintOrderInfo(ordersInfo);
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(separator + "\n");
        }

        private static void PrintTask2(string separator, string orderInfoSqlExpression, DatabaseManager databaseManager)
        {
            Console.WriteLine(separator);
            ColorConsole.WriteLine("Task_2. Print all orders for the last year using SqlDataAdapter/DataSet:\n",
                ConsoleColor.Green);

            try
            {
                var ordersInfo = databaseManager.GetOrdersViaSqlDataAdapter(orderInfoSqlExpression);
                PrintOrderInfo(ordersInfo);
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(separator + "\n");
        }

        private static void PrintTask3(string separator, DatabaseManager databaseManager)
        {
            Console.WriteLine(separator);
            ColorConsole.WriteLine("Task_3. Create a new order using SqlCommand:\n", ConsoleColor.Green);

            var newOrder = new OrdersEntity
            {
                OrderDateTime = DateTime.Now,
                AnalysisId = 3
            };

            try
            {
                newOrder.Id = databaseManager.CreateOrder(newOrder);
                OrdersEntity? order = databaseManager.GetOrderById(newOrder.Id);

                if (order is not null)
                {
                    Console.WriteLine("[Create 1 order]\n");
                    Console.WriteLine($"Id\tOrderDateTime\t\tAnalysisId\t\n");
                    Console.WriteLine($"{order.Id}\t{order.OrderDateTime}\t{order.AnalysisId}");
                }
                else
                {
                    ColorConsole.WriteLine("Failed to create a new order", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(separator + "\n");
        }

        private static void PrintTask4(string separator, DatabaseManager databaseManager)
        {
            Console.WriteLine(separator);
            ColorConsole.WriteLine("Task_4. Update one (last) order using SqlCommand:\n", ConsoleColor.Green);

            try
            {
                var lastOrder = databaseManager.GetLastOrder();
                if (lastOrder is null)
                {
                    ColorConsole.WriteLine("No orders have been found", ConsoleColor.Blue);
                    return;
                }

                Console.WriteLine($"Status\tId\tOrderDateTime\t\tAnalysisId\t\n");
                Console.WriteLine($"Old ->\t{lastOrder.Id}\t{lastOrder.OrderDateTime}\t{lastOrder.AnalysisId}");
                Console.WriteLine("\t|\n\t|\n\t|");

                lastOrder.AnalysisId = 5;

                int count = databaseManager.UpdateOrder(lastOrder);

                lastOrder = databaseManager.GetLastOrder()!;
                Console.WriteLine($"New ->\t{lastOrder.Id}\t{lastOrder.OrderDateTime}\t{lastOrder.AnalysisId}\n");

                Console.WriteLine($"[Updated: {count} order]");
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(separator + "\n");
        }

        private static void PrintTask5(string separator, DatabaseManager databaseManager)
        {
            Console.WriteLine(separator);
            ColorConsole.WriteLine("Task_5. Delete one (last) order using SqlCommand:\n", ConsoleColor.Green);

            try
            {
                var lastOrder = databaseManager.GetLastOrder();
                if (lastOrder is null)
                {
                    ColorConsole.WriteLine("No orders have been found", ConsoleColor.Blue);
                    return;
                }

                int count = databaseManager.DeleteOrder(lastOrder.Id);
                Console.WriteLine($"[Deleted: {count} order]");
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(separator + "\n");
        }

        private static void PrintOrderInfo(IEnumerable<OrderInfo>? ordersInfo)
        {
            if (ordersInfo is not null)
            {
                Console.WriteLine($"[{ordersInfo.Count()} records found]\n");
                Console.WriteLine($"Id\tOrderDateTime\t\tPrice\t\tName\t\t\t\tGroupName\n");

                foreach (var orderInfo in ordersInfo)
                {
                    Console.WriteLine($"{orderInfo.Id}\t{orderInfo.OrderDateTime}\t" +
                        $"${orderInfo.Price:F2}\t\t{orderInfo.Name}\t\t\t{orderInfo.GroupName}");
                }
            }
            else
            {
                ColorConsole.WriteLine("No orders have been found", ConsoleColor.Blue);
            }
        }
    }
}
