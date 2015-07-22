namespace SupermarketsChain
{
    using System;
    using System.IO;
    using System.Linq;

    using NPOI.SS.UserModel;
    using NPOI.HSSF.UserModel;

    class XlsFileReader
    {
        public static string DateOrder;

        public static void ReadXls(string fileAndPath)
        {
            HSSFWorkbook xlsFile;

            string[] pathParts = fileAndPath.Split('\\');
            string saleDate = pathParts[pathParts.Count() - 2];

            using (FileStream file = new FileStream(fileAndPath, FileMode.Open, FileAccess.Read))
            {
                xlsFile = new HSSFWorkbook(file);
            }

            ISheet sheet = xlsFile.GetSheet("Sales");

            CollectData(sheet);
        }

        public static void CollectData(ISheet sheet)
        {
            var dbContext = new SupermarketsChainEntities();

            var vendor = new Vendor();
            var product = new Product();
            var sale = new Sale();
            var supermarket = new Supermarket();

            int quantity;
            int measureType = 1;
            string cellValue;

            int cell = 1;
            var saleCell = new Sale();
            for (int row = 1; row < sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null)
                {
                    
                    while (sheet.GetRow(row).GetCell(cell) != null && sheet.GetRow(row).GetCell(cell).ToString() != "")
                    {
                        cellValue = sheet.GetRow(row).GetCell(cell).ToString();
                        
                        //saleCell.Date = sheet.GetRow(row).GetCell(1).ToString();
                        //(int) saleCell.Quantity = sheet.GetRow(row).GetCell(2).ToString();
                        //saleCell.ProductId = db.Products.FirstOrDefault(x => x.Name == sheet.GetRow(row).GetCell(1)).Id;
                        //db.Sales.Add(saleCell);
                        //db.SaveChanges();
                        //Console.WriteLine(String.Format("Row: {0}, Col: {1}, Data: {2}", row, cell, cellValue));
                        if (row == 1 && cell == 1)
                        {
                            vendor.Name = cellValue;
                            var supermarketName = vendor.Name.Substring(13);
                            supermarketName = supermarketName.Substring(0, supermarketName.Length - 1);
                            var supermarketTemp = dbContext.Supermarkets
                                    .Where(s => s.Name == supermarketName)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
                            //Console.WriteLine("Supermarket Id = " + supermarketTemp);
                            saleCell.SupermarketId = supermarketTemp;
                        }
                        if (row > 2 && cellValue != null)
                        {
                            if (cell == 1)
                            {
                                product.Name = cellValue;
                                var productId = dbContext.Products
                                    .Where(p => p.Name == product.Name)
                                    .Select(p => p.Id)
                                    .FirstOrDefault();
                                //Console.WriteLine("Product Id = " + productId);
                                saleCell.ProductId = productId;
                            }
                            if (cell == 2)
                            {
                                quantity = int.Parse(cellValue);
                                for (int quantityLines = 0; quantityLines < quantity; quantityLines++)
                                {
                                    var newCell = new Sale();
                                    newCell.ProductId = saleCell.ProductId;
                                    newCell.SupermarketId = saleCell.SupermarketId;
                                    newCell.OrderedOn = DateTime.Parse(DateOrder);
                                    dbContext.Sales.Add(newCell);
                                    //Console.WriteLine("Quantity: 1");
                                }

                                dbContext.SaveChanges();
                            }
                            if (cell == 3)
                            {
                                product.Price = decimal.Parse(cellValue);
                            }
                        }

                        cell++;
                    }
                }

                product.Name = "";
                cell = 1;
            }
        }


        public static DateTime SaleDate(string saleDate)
        {
            var dateTime = Convert.ToDateTime(saleDate);

            return dateTime;
        }
    }
}
