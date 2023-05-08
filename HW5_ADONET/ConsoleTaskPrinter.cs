using HW_ADONET__EFCore.DatabaseHelpers.Entities;
using HW_ADONET__EFCore.DatabaseHelpers;
using HW_ADONET__EFCore.DatabaseHelpers.AdoNetHelpers;
using HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers;

namespace HW_ADONET__EFCore
{
    internal class ConsoleTaskPrinter
    {
        private static readonly string _separator;
        private readonly IDatabaseManager _databaseManager;

        static ConsoleTaskPrinter()
        {
            _separator = new string('-', Console.WindowWidth);
        }

        public ConsoleTaskPrinter(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;

            PrintWelcomeMessage();
        }

        public void PrintAllLastYearOrders()
        {
            Console.WriteLine(_separator);
            ColorConsole.WriteLine("Print all orders for the last year:\n", ConsoleColor.Green);

            try
            {
                var ordersInfo = _databaseManager.GetLastYearOrdersInfo();

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
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(_separator + "\n");
        }

        public void PrintCreatedOrder()
        {
            Console.WriteLine(_separator);
            ColorConsole.WriteLine("Create a new order:\n", ConsoleColor.Green);

            var newOrder = new OrderEntity
            {
                OrderDateTime = DateTime.Now,
                AnalysisId = 3
            };

            try
            {
                newOrder.Id = _databaseManager.CreateOrder(newOrder);
                OrderEntity? order = _databaseManager.GetOrderById(newOrder.Id);

                if (order is not null)
                {
                    Console.WriteLine("[Create 1 order]\n");
                    PrintFormattedOrderEntity(order);
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

            Console.WriteLine(_separator + "\n");
        }

        public void PrintUpdatedOrder()
        {
            Console.WriteLine(_separator);
            ColorConsole.WriteLine("Update one (last) order:\n", ConsoleColor.Green);

            try
            {
                var lastOrder = _databaseManager.GetLastOrder();
                if (lastOrder is null)
                {
                    ColorConsole.WriteLine("No orders have been found to update", ConsoleColor.Blue);
                    return;
                }


                lastOrder.AnalysisId = 5;
                int count = _databaseManager.UpdateOrder(lastOrder);

                if (count > 0)
                {
                    Console.WriteLine($"[Updated: {count} order]\n");

                    Console.WriteLine($"Status\tId\tOrderDateTime\t\tAnalysisId\t\n");
                    Console.WriteLine($"Old ->\t{lastOrder.Id}\t{lastOrder.OrderDateTime}\t{lastOrder.AnalysisId}");
                    Console.WriteLine("\t|\n\t|\n\t|");

                    lastOrder = _databaseManager.GetLastOrder()!;
                    Console.WriteLine($"New ->\t{lastOrder.Id}\t{lastOrder.OrderDateTime}\t{lastOrder.AnalysisId}");
                }
                else
                {
                    ColorConsole.WriteLine("Failed to update the order", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(_separator + "\n");
        }

        public void PrintDeletedOrder()
        {
            Console.WriteLine(_separator);
            ColorConsole.WriteLine("Delete one (last) order:\n", ConsoleColor.Green);

            try
            {
                var lastOrder = _databaseManager.GetLastOrder();
                if (lastOrder is null)
                {
                    ColorConsole.WriteLine("No orders have been found to delete", ConsoleColor.Blue);
                    return;
                }

                int count = _databaseManager.DeleteOrder(lastOrder.Id);

                if (count > 0)
                {
                    Console.WriteLine($"[Deleted: {count} order]\n");
                    PrintFormattedOrderEntity(lastOrder);
                }
                else
                {
                    ColorConsole.WriteLine("Failed to delete the order", ConsoleColor.Red);
                }
            }
            catch (Exception ex)
            {
                ColorConsole.WriteLine($"Exception! {ex.Message}", ConsoleColor.Red);
            }

            Console.WriteLine(_separator + "\n");
        }

        private void PrintWelcomeMessage()
        {
            string dataAcessFramework = string.Empty;

            if (_databaseManager is AdoNetDatabaseManager)
            {
                dataAcessFramework = "ADO.NET";
            }
            else if (_databaseManager is EFCoreDatabaseManager)
            {
                dataAcessFramework = "Entity Framework Core";
            }

            if (dataAcessFramework != string.Empty)
            {
                string welcomeMessage = $"SQL queries are performed using {dataAcessFramework}"; 

                Console.CursorLeft = (Console.WindowWidth / 2) - (welcomeMessage.Length / 2); 

                ColorConsole.WriteLine(welcomeMessage + "\n", ConsoleColor.Magenta);
            }       
        }

        private void PrintFormattedOrderEntity(OrderEntity orderEntity)
        {
            Console.WriteLine($"Id\tOrderDateTime\t\tAnalysisId\t\n");
            Console.WriteLine($"{orderEntity.Id}\t{orderEntity.OrderDateTime}\t{orderEntity.AnalysisId}");
        }
    }
}
