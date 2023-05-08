using HW_ADONET__EFCore.DatabaseHelpers.AdoNetHelpers;
using HW_ADONET__EFCore.DatabaseHelpers.EFCoreHelpers;

namespace HW_ADONET__EFCore
{
    internal class Starter
    {
        public static void Run()
        {
            string? connectionString = ConfigurationHelper.GetConnectionString();
            if (connectionString is null)
            {
                ColorConsole.WriteLine("Incorrect connection string or missing", ConsoleColor.Red);
                return;
            }

            var databaseManager = new EFCoreDatabaseManager(connectionString);
            //var databaseManager = new AdoNetDatabaseManager(connectionString);

            var taskPrinter = new ConsoleTaskPrinter(databaseManager);

            taskPrinter.PrintAllLastYearOrders();
            taskPrinter.PrintCreatedOrder();
            taskPrinter.PrintUpdatedOrder();
            taskPrinter.PrintDeletedOrder();
        }
    }
}
