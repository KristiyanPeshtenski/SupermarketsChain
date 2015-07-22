namespace SupermarketsChain
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public static class OracleDBToSQLServer
    {
        private static OracleEntities dbOracleContext = new OracleEntities();
        private static SupermarketsChainEntities dbContext = new SupermarketsChainEntities();

        public static void ReplicateDataFromOracle()
        {
            ReplicateVendors();
            ReplicateMeasures();
            ReplicateProducts();
            ReplicateSupplyedSupermarkets();
        }

        private static void ReplicateVendors()
        {
            var inputVendors = dbOracleContext.VENDORS;
            var outputVendors = dbContext.Vendors;

            Console.Write("Replicate vendors: ");
            foreach (var vendor in inputVendors)
            {
                var newVendor = new Vendor()
                {
                    Name = vendor.NAME
                };

                outputVendors.AddOrUpdate(v => v.Name, newVendor);
            }

            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void ReplicateMeasures()
        {
            var inputMeasures = dbOracleContext.MEASURES;
            var outputMeasures = dbContext.Measures;

            Console.Write("Replicate measures: ");
            foreach (var measure in inputMeasures)
            {
                var newMeasure = new Measure()
                {
                    Name = measure.NAME
                };

                outputMeasures.AddOrUpdate(m => m.Name, newMeasure);
            }

            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void ReplicateProducts()
        {
            var inputProducts = dbOracleContext.PRODUCTS;
            var outputProducts = dbContext.Products;

            Console.Write("Replicate products: ");
            foreach (var product in inputProducts)
            {
                var newProduct = new Product()
                {
                    Name = product.NAME,
                    VendorId = product.VENDORID,
                    MeasureId = product.MEASUREID,
                    Price = product.PRICE
                };
                

                outputProducts.AddOrUpdate(p => new { p.Name, p.VendorId, p.Price }, newProduct);
            }

            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }

        private static void ReplicateSupplyedSupermarkets()
        {
            var inputSupermarkets = dbOracleContext.SUPERMARKETS;
            var outputSupermarkets = dbContext.Supermarkets;

            Console.Write("Replicate supermarkets and supply them with products: ");
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

                outputSupermarkets.AddOrUpdate(s => s.Name, newSupermarket);
            }

            dbContext.SaveChanges();
            Console.WriteLine("Done!");
        }
    }
}
