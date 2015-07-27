using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;

namespace SupermarketChain
{
    public class OracleToSqlServer
    {
        private static readonly SupermarketChainOracleEntities DbOracleContext = new SupermarketChainOracleEntities();
        private static readonly SupermarketsChainSqlServerEntities DbContext = new SupermarketsChainSqlServerEntities();
        private static readonly StringBuilder Output = new StringBuilder();

        public string ReplicateDataFromOracle()
        {
            var consoleOutput = new StringBuilder();
            consoleOutput.Clear();
            consoleOutput.Append(this.ReplicateVendors())
                .Append(this.ReplicateMeasures())
                .Append(this.ReplicateProducts())
                .Append(this.ReplicateSupplyedSupermarkets());
            return consoleOutput.ToString();
        }

        private string ReplicateVendors()
        {
            Output.Clear();
            var inputVendors = DbOracleContext.VENDORS;
            var outputVendors = DbContext.Vendors;
            Output.Append("Replicate vendors: ");
            foreach (var vendor in inputVendors)
            {
                var newVendor = new Vendors()
                {
                    Name = vendor.NAME
                };

                outputVendors.AddOrUpdate(v => v.Name, newVendor);
            }

            DbContext.SaveChanges();
            Output.AppendLine("Done!");
            return Output.ToString();
        }

        private string ReplicateMeasures()
        {
            Output.Clear();
            var inputMeasures = DbOracleContext.MEASURES;
            var outputMeasures = DbContext.Measures;

            Output.Append("Replicate measures: ");
            foreach (var measure in inputMeasures)
            {
                var newMeasure = new Measures()
                {
                    Name = measure.NAME
                };

                outputMeasures.AddOrUpdate(m => m.Name, newMeasure);
            }

            DbContext.SaveChanges();
            Output.AppendLine("Done!");
            return Output.ToString();
        }

        private string ReplicateProducts()
        {
            Output.Clear();
            var inputProducts = DbOracleContext.PRODUCTS;
            var outputProducts = DbContext.Products;

            Output.Append("Replicate products: ");
            foreach (var product in inputProducts)
            {
                var newProduct = new Products()
                {
                    Name = product.NAME,
                    VendorId = product.VENDORID,
                    MeasureId = product.MEASUREID,
                    Price = product.PRICE
                };


                outputProducts.AddOrUpdate(p => new { p.Name, p.VendorId, p.Price }, newProduct);
            }

            DbContext.SaveChanges();
            Output.AppendLine("Done!");
            return Output.ToString();
        }

        private string ReplicateSupplyedSupermarkets()
        {
            Output.Clear();
            var inputSupermarkets = DbOracleContext.SUPERMARKETS;
            var outputSupermarkets = DbContext.Supermarkets;

            Output.Append("Replicate supermarkets and supply them with products: ");
            foreach (var supermarket in inputSupermarkets)
            {
                var newSupermarket = new Supermarkets()
                {
                    Name = supermarket.NAME
                };

                foreach (var product in supermarket.PRODUCTS)
                {
                    newSupermarket.Products.Add(DbContext.Products.FirstOrDefault(p => p.Id == product.ID));
                }

                outputSupermarkets.AddOrUpdate(s => s.Name, newSupermarket);
            }

            DbContext.SaveChanges();
            Output.Append("Done!");
            return Output.ToString();
        }
    }
}
