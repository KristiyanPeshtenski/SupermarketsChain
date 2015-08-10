namespace SupermarketsChain
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Script.Serialization;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class SQLServerToJSONToMongoDB
    {
        public static void Main()
        {
            var dbContext = new SupermarketsChainEntities();
            var startDate = Convert.ToDateTime("07/27/2015");
            var endDate = Convert.ToDateTime("07/27/2015");
            var products = dbContext.Products
                .Where(
                    p => p.Sales
                             .Any(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate))
                .Select(
                    p => new
                             {
                                 ProductId = p.Id,
                                 ProductName = p.Name,
                                 VendorName = p.Vendor.Name,
                                 TotalQuantitySold = p.Sales.Count(s => s.ProductId == p.Id),
                                 TotalIncomes = p.Price * p.Sales.Count(s => s.ProductId == p.Id)
                             });

            JavaScriptSerializer js = new JavaScriptSerializer();

            foreach (var product in products)
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

                string path = "..\\..\\Json-Reports\\" + product.ProductId + ".json";
                File.AppendAllText(path, json);

                var connectionString = "mongodb://localhost";
                var client = new MongoClient(connectionString);
                var server = client.GetServer();
                server.Connect();
                var database = server.GetDatabase("local");
                var collection = database.GetCollection<BsonDocument>("SalesByProductReports");

                BsonDocument bson = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(json);
                collection.Insert(bson);
            }  
        }
    }
}
