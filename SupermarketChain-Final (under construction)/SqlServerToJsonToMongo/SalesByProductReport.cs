using System.Text;

namespace SupermarketChain
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class SalesByProductReport
    {
        private static readonly JavaScriptSerializer Js = new JavaScriptSerializer(); 

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string VendorName { get; set; }

        public int TotalQuantitySold { get; set; }

        public decimal TotalIncomes { get; set; }

        private string ExportToFolder(DateTime startDate, DateTime endDate)
        {
            var products = GetProductDataForReport(startDate, endDate);

            string directoryPath = "..\\..\\Json-Reports\\";
            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (var product in products)
            {
                var json = GenerateJsonObjectForProduct(product, Js);

                string filePath = directoryPath + product.ProductId + ".json";
                File.AppendAllText(filePath, json);
            }

            return "Exporting JSON Product Report to folder: Done!";
        }

        private string ExportToServer(DateTime startDate, DateTime endDate)
        {
            var products = GetProductDataForReport(startDate, endDate);

            var connectionString = "mongodb://localhost";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            server.Connect();
            var database = server.GetDatabase("local");

            string collectionName = "SalesByProductReports";
            if (!database.CollectionExists(collectionName))
            {
                database.CreateCollection(collectionName);
            }

            var collection = database.GetCollection<BsonDocument>(collectionName);
            collection.RemoveAll();

            foreach (var product in products)
            {
                var json = GenerateJsonObjectForProduct(product, Js);

                BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);
                collection.Insert(bson);
            }

            return "Exporting JSON Product Report to Mongo DB: Done!";
        }

        public string ExportJson(DateTime startDate, DateTime endDate)
        {
            var output = new StringBuilder();
            output.AppendLine(this.ExportToFolder(startDate, endDate));
            output.AppendLine(this.ExportToServer(startDate, endDate));
            return output.ToString();
        }

        private static string GenerateJsonObjectForProduct(SalesByProductReport product, JavaScriptSerializer js)
        {
            var salesByProductReport = new SalesByProductReport
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                VendorName = product.VendorName,
                TotalQuantitySold = product.TotalQuantitySold,
                TotalIncomes = product.TotalIncomes
            };
            var json = js.Serialize(salesByProductReport);
            return json;
        }

        private static IQueryable<SalesByProductReport> GetProductDataForReport(DateTime startDate, DateTime endDate)
        {
            var dbContext = new SupermarketsChainSqlServerEntities();
            var products = dbContext.Products
                .Where(
                    p => p.Sales
                        .Any(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate))
                .Select(
                    p => new SalesByProductReport
                    {
                        ProductId = p.Id,
                        ProductName = p.Name,
                        VendorName = p.Vendors.Name,
                        TotalQuantitySold = p.Sales.Count(s => s.ProductId == p.Id),
                        TotalIncomes = p.Price*p.Sales.Count(s => s.ProductId == p.Id)
                    });
            return products;
        }
    }
}
