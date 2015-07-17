namespace SupermarketsChain
{
    using System;
    using System.Linq;

    public static class OracleDBToSQLServer
    {
        private static OracleEntities dbOracleContext = new OracleEntities();
        private static SupermarketsChainEntities dbContext = new SupermarketsChainEntities();

        public static void MoveAllData()
        {
            MoveVendors();
            MoveMeasures();
            MoveProducts();
            MoveSupplyedSupermarkets();
        }

        private static void MoveVendors()
        {
            var inputVendors = dbOracleContext.VENDORS;
            var outputVendors = dbContext.Vendors;

            Console.Write("Move vendors: ");
            foreach (var vendor in inputVendors)
            {
                var newVendor = new Vendor()
                {
                    Name = vendor.NAME
                };

                outputVendors.Add(newVendor);
            }
            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void MoveMeasures()
        {
            var inputMeasures = dbOracleContext.MEASURES;
            var outputMeasures = dbContext.Measures;

            Console.Write("Move measures: ");
            foreach (var measure in inputMeasures)
            {
                var newMeasure = new Measure()
                {
                    Name = measure.NAME
                };

                outputMeasures.Add(newMeasure);
            }
            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void MoveProducts()
        {
            var inputProducts = dbOracleContext.PRODUCTS;
            var outputProducts = dbContext.Products;

            Console.Write("Move products: ");
            foreach (var product in inputProducts)
            {
                var newProduct = new Product()
                {
                    Name = product.NAME,
                    VendorId = product.VENDORID,
                    MeasureId = product.MEASUREID,
                    Price = product.PRICE
                };
                

                outputProducts.Add(newProduct);
            }
            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void MoveSupplyedSupermarkets()
        {
            var inputSupermarkets = dbOracleContext.SUPERMARKETS;
            var outputSupermarkets = dbContext.Supermarkets;

            Console.Write("Move supermarkets and supply them with products: ");
            foreach (var supermarket in inputSupermarkets)
            {
                var newSupermarket = new Supermarket()
                {
                    Name = supermarket.NAME
                };

                foreach (var product in supermarket.PRODUCTS)
                {
                    newSupermarket.Products.Add(dbContext.Products.FirstOrDefault(p => p.Id == product.ID));
                }

                outputSupermarkets.Add(newSupermarket);
            }
            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }
    }
}
