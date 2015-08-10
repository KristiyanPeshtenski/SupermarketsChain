namespace SupermarketChain
{
    using System;
    using System.IO;
    using System.Linq;
    using NPOI.HSSF.UserModel;
    using NPOI.SS.UserModel;

    public class XlsFileReader
    {
        public string DateOrder;

        public void ReadXls(string fileAndPath)
        {
            HSSFWorkbook xlsFile;

            string[] pathParts = fileAndPath.Split('\\');
            string saleDate = pathParts[pathParts.Count() - 2];

            using (FileStream file = new FileStream(fileAndPath, FileMode.Open, FileAccess.Read))
            {
                xlsFile = new HSSFWorkbook(file);
            }

            ISheet sheet = xlsFile.GetSheet("Sales");

            this.CollectData(sheet);
        }

        public void CollectData(ISheet sheet)
        {
            var dbContext = new SupermarketsChainSqlServerEntities();

            var vendor = new Vendors();
            var product = new Products();
            var sale = new Sales();
            var supermarket = new Supermarkets();

            int quantity;
            string cellValue;

            int cell = 1;
            var saleCell = new Sales();
            for (int row = 1; row < sheet.LastRowNum; row++)
            {
                if (sheet.GetRow(row) != null)
                {

                    while (sheet.GetRow(row).GetCell(cell) != null && sheet.GetRow(row).GetCell(cell).ToString() != string.Empty)
                    {
                        cellValue = sheet.GetRow(row).GetCell(cell).ToString();

                        if (row == 1 && cell == 1)
                        {
                            vendor.Name = cellValue;
                            var supermarketName = vendor.Name.Substring(13);
                            supermarketName = supermarketName.Substring(0, supermarketName.Length - 1);
                            var supermarketTemp = dbContext.Supermarkets
                                    .Where(s => s.Name == supermarketName)
                                    .Select(s => s.Id)
                                    .FirstOrDefault();
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
                                saleCell.ProductId = productId;
                            }
                            if (cell == 2)
                            {
                                quantity = int.Parse(cellValue);
                                for (int quantityLines = 0; quantityLines < quantity; quantityLines++)
                                {
                                    var newCell = new Sales
                                                      {
                                                          ProductId = saleCell.ProductId,
                                                          SupermarketId = saleCell.SupermarketId,
                                                          OrderedOn = DateTime.Parse(this.DateOrder)
                                                      };
                                    dbContext.Sales.Add(newCell);
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

                product.Name = string.Empty;
                cell = 1;
            }
        }
    }
}
