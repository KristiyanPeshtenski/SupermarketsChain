using System;
using System.Data.SQLite;
using SQL = System.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace SQLiteEntities
{
    class Product
    {
        public Product(string name, double tax)
        {
            this.Name = name;
            this.Tax = tax;
        }

        public double Tax { get; set; }

        public string Name { get; set; }
    }
    
    class ProjectMain
    {
        static void Main()
        {
            var p = new ProjectMain();
            const string sqlSelect = "SELECT * FROM ProductTaxes";
            const string sqlInsert = "INSERT INTO ProductTaxes VALUES (\"SoftUni\", 20)";
            const string dataSource = "E:/Programming/Database Apps/TaxInformation.sqlite";
            var product = new Product("softuni", 20);
            var con = new SQLiteConnection("Data Source=" + dataSource);
            var selectCommand = new SQLiteCommand(sqlSelect, con);
            var insertCommand = new SQLiteCommand(sqlInsert, con);
            try
            {
                con.Open();
                insertCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                con.Close();
            }
            SQLiteDataReader reader = null;
            var productsTaxes = new SQL.DataTable("asfd");
            productsTaxes.Columns.Add("Product");
            productsTaxes.Columns.Add("Tax");

            try
            {
                con.Open();
                reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    productsTaxes.Rows.Add(reader["ProductName"].ToString(), reader["Tax"].ToString());
                }
            }
            catch (Exception)
            {
                Console.WriteLine("exception thrown!");
            }
            finally
            {
                if (reader != null) reader.Close();
                con.Close();
            }

            SQL.DataSet ds = new SQL.DataSet("Organization");
            ds.Tables.Add(productsTaxes);
            p.ExportDataSetToExcel(ds);
        }

        private void ExportDataSetToExcel(SQL.DataSet ds)
        {
            //Creae an Excel application instance
            Excel.Application excelApp = new Excel.Application();

            //Create an Excel workbook instance and open it from the predefined location
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Open(@"E:\Programming\Database Apps\report.xlsx");

            foreach (SQL.DataTable table in ds.Tables)
            {
                //Add a new worksheet to workbook with the Datatable name
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }

            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApp.Quit();
        }
    }
}
