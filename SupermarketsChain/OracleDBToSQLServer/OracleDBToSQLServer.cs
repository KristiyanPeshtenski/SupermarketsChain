namespace SupermarketsChain
{
    using System;

    class OracleDBToSQLServer
    {
        public static void Main()
        {
            var dbOracleContext = new OracleEntities();
            var dbContext = new SupermarketsChainEntities();

            var inputVendors = dbOracleContext.VENDORS;
            var vendors = dbContext.Vendors;

            Console.WriteLine("Oracle vendors:");
            foreach (var vendor in inputVendors)
            {
                Console.WriteLine(vendor.NAME);
            }

            Console.WriteLine("\nMSSQL vendors:");
            foreach (var vendor in vendors)
            {
                Console.WriteLine(vendor.Name);
            }
        }
    }
}
