namespace SupermarketsChain
{
    using System;

    class ZIPArchiveToSQLServer
    {
        static void Main()
        {
            var db = new SupermarketsChainEntities();
            //string filePath = @"C:\Users\Emily\Documents\SoftUni\DatabaseApplications\Teamwork\Repo\Sources\Sales-Reports.zip";
            string filePath = @"..\..\..\..\Sources\Sales-Reports.zip";
            ZipFileReader.ReadZipFile(filePath);
            var context = new SupermarketsChainEntities();

            //var productName = Console.ReadLine(); // domati
            //var product = context.Products.FirstOrDefault(p => p.Name == productName); // PRoduct

            //if (product == null)
            //{
            //    product = new Product() {Name = productName};
            //    context.Products.Add(product);
            //    context.SaveChanges();
            //}

            //var novZapis = new Sale()
            //{
            //    OrderedOn = DateTime.Now,
            //    ProductId = product.Id,
            //};

            //context.Sales.Add(novZapis);
            //context.SaveChanges();
            Console.WriteLine("Done.");
        }
    }
}
