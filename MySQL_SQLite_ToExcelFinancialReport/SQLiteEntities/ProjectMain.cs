using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
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

    class Sales
    {
        public Sales(string name, decimal income, decimal expense, decimal tax)
        {
            this.Name = name;
            this.Income = income;
            this.Expense = expense;
            this.Tax = tax;
        }

        public string Name { get; set; }

        public decimal Income { get; set; }

        public decimal Expense { get; set; }

        public decimal Tax { get; set; }
    }
    
    class ProjectMain
    {
        static void Main()
        {
            var p = new ProjectMain();
            const string sqlSelect = "SELECT * FROM ProductTaxes";
            const string dataSource = "C:/Users/simeo_000/Desktop/TaxInformation.sqlite";
            var product = new Product("softuni", 20);
            var con = new SQLiteConnection("Data Source=" + dataSource);
            var selectCommand = new SQLiteCommand(sqlSelect, con);
            var sales = new List<Sales>();
            SQLiteDataReader reader = null;
            var productsTaxes = new SQL.DataTable("asfd");
            productsTaxes.Columns.Add("Vendor");
            productsTaxes.Columns.Add("Incomes");
            productsTaxes.Columns.Add("Expenses");
            productsTaxes.Columns.Add("Total taxes");
            productsTaxes.Columns.Add("Financial result");
            String myConn = "server=localhost;Database=supermarkets_chain;uid=root;pwd=;";
            MySqlConnection conn = new MySqlConnection(myConn);
            String selectData = "select v.name as Vendor, p.name as Product, ifnull((select sum(p.price) from products p join vendors vv on p.vendor_id = vv.id join sales s on s.product_id = p.id where vv.Id = v.Id), 0) as Incomes, ifnull((select sum(e.expense) from vendors vvv join vendors_has_expenses ve on vvv.id = ve.vendor_id join expenses e on e.id = ve.expense_id where vvv.id = v.id), 0) as Expenses from vendors v join products p on p.vendor_id = v.id group by v.id, v.name, p.name";
            MySqlCommand cmdData = new MySqlCommand(selectData, conn);
            MySqlDataReader dataReader = null;
            try
            {
                con.Open();
                reader = selectCommand.ExecuteReader();
                conn.Open();
                dataReader = cmdData.ExecuteReader();

                while (reader.Read() && dataReader.Read())
                {
                    var tax = Convert.ToDecimal(reader["Tax"].ToString());
                    var income = dataReader.GetDecimal(2);
                    var taxPercentage = tax/100;
                    var currentTax = income*taxPercentage;
                    var sale = new Sales(dataReader.GetString(0),
                        income,
                        dataReader.GetDecimal(3),
                        currentTax);
                    sales.Add(sale);
                }
                var groupedSales = sales
                    .GroupBy(s => new {s.Name, s.Expense, s.Income})
                    .Select(s => new
                    {
                        VendorName = s.Key.Name,
                        Expense = s.Key.Expense,
                        Income = s.Key.Income,
                        FinancialResult = (s.Key.Income - s.Key.Expense - s.Sum(s1 => s1.Tax)).ToString(new CultureInfo("en-US")),
                        Tax = s.Sum(s1 => s1.Tax).ToString(new CultureInfo("en-US")),
                    })
                    .ToList();
                
                groupedSales.ForEach(gs => productsTaxes.Rows.Add(
                        gs.VendorName,
                        gs.Income,
                        gs.Expense,
                        gs.Tax,
                        gs.FinancialResult
                        ));
                
            }
            catch (Exception)
            {
                Console.WriteLine("exception thrown!");
            }
            finally
            {
                if (reader != null) reader.Close();
                if (reader != null) dataReader.Close();
                con.Close();
            }


            SQL.DataSet ds = new SQL.DataSet("Organization");
            ds.Tables.Add(productsTaxes);
            ExportDataSetToExcel(ds);
        }

        private static void ExportDataSetToExcel(SQL.DataSet ds)
        {
            //Creae an Excel application instance
            var excelApp = new Excel.Application();
            //Create an Excel workbook instance and open it from the predefined location
            if (System.IO.File.Exists(@"C:\Users\simeo_000\Desktop\Report.xlsx"))
            {
                System.IO.File.Delete(@"C:\Users\simeo_000\Desktop\Report.xlsx");
            }
            var excelWorkBook = excelApp.Workbooks.Add(Missing.Value);

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

            excelWorkBook.SaveAs(@"C:\Users\simeo_000\Desktop\Report.xlsx");

            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApp.Quit();
        }
    }
}