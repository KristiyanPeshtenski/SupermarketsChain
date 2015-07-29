namespace SupermarketChain
{
    using System;
    using System.Collections.Generic;
    using System.Data.SQLite;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;

    using MySql.Data.MySqlClient;

    using Excel = Microsoft.Office.Interop.Excel;
    using SQL = System.Data;

    public class SqliteMysqlToExcel
    {
        private const string SQLiteDataSource = @"E:\Programming\Database Apps\TaxInformation.sqlite";
        private const string ExportToExcelSuccess = "Export excel financial result by vendor: Done!";
        private readonly string exportFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Report.xlsx";

        public string ExportDataSetToExcel()
        {
            SQL.DataSet ds;

            try
            {
                ds = this.PrepareDataForExcelExport();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            var excelApp = new Excel.Application();
            var excelPath = this.exportFileName;

            if (System.IO.File.Exists(excelPath))
            {
                System.IO.File.Delete(excelPath);
            }

            var excelWorkBook = excelApp.Workbooks.Add(Missing.Value);

            foreach (SQL.DataTable table in ds.Tables)
            {
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

            excelWorkBook.SaveAs(excelPath);

            excelWorkBook.Save();
            excelWorkBook.Close();
            excelApp.Quit();

            return ExportToExcelSuccess;
        }

        private SQL.DataSet PrepareDataForExcelExport()
        {
            const string SqlSelect = "SELECT * FROM ProductTaxes";
            var sqLiteConnection = new SQLiteConnection("Data Source=" + SQLiteDataSource);
            var selectCommand = new SQLiteCommand(SqlSelect, sqLiteConnection);
            var sales = new List<SaleForExcelReport>();
            SQLiteDataReader sqLiteDataReader = null;

            var productsTaxes = new SQL.DataTable("asfd");
            productsTaxes.Columns.Add("Vendor");
            productsTaxes.Columns.Add("Incomes");
            productsTaxes.Columns.Add("Expenses");
            productsTaxes.Columns.Add("Total taxes");
            productsTaxes.Columns.Add("Financial result");

            var myConn = "server=localhost;Database=supermarkets_chain;uid=root;pwd=;";
            var mySqlConnection = new MySqlConnection(myConn);
            var selectData = @"select 
                                    v.name as Vendor, 
                                    p.name as Product, 
                                    ifnull((select sum(p.price) 
                                            from products p 
                                            join vendors vv on p.vendor_id = vv.id 
                                            join sales s on s.product_id = p.id where vv.Id = v.Id), 0) as Incomes, 
                                    ifnull((select sum(e.expense) 
                                            from vendors vvv join vendors_has_expenses ve on vvv.id = ve.vendor_id 
                                            join expenses e on e.id = ve.expense_id where vvv.id = v.id), 0) as Expenses 
                                    from vendors v 
                                    join products p on p.vendor_id = v.id 
                                    group by v.id, v.name, p.name";
            var cmdData = new MySqlCommand(selectData, mySqlConnection);
            MySqlDataReader mySqlDataReader = null;

            try
            {
                sqLiteConnection.Open();
                sqLiteDataReader = selectCommand.ExecuteReader();
                mySqlConnection.Open();
                mySqlDataReader = cmdData.ExecuteReader();

                while (sqLiteDataReader.Read() && mySqlDataReader.Read())
                {
                    this.AddDataIntoSalesList(sqLiteDataReader, mySqlDataReader, sales);
                }

                this.AddSalesIntoDataTable(sales, productsTaxes);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (sqLiteDataReader != null)
                {
                    sqLiteDataReader.Close();
                }

                if (mySqlDataReader != null)
                {
                    mySqlDataReader.Close();
                }

                sqLiteConnection.Close();
            }


            SQL.DataSet ds = new SQL.DataSet("Organization");
            ds.Tables.Add(productsTaxes);
            return ds;
        }

        private void AddSalesIntoDataTable(List<SaleForExcelReport> sales, SQL.DataTable productsTaxes)
        {
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
                gs.FinancialResult));
        }

        private void AddDataIntoSalesList(SQLiteDataReader sqLiteDataReader, MySqlDataReader mySqlDataReader, List<SaleForExcelReport> sales)
        {
            var productTax = Convert.ToDecimal(sqLiteDataReader["Tax"].ToString());
            var income = mySqlDataReader.GetDecimal(2);
            var taxPercentage = productTax / 100;
            var currentTax = income * taxPercentage;
            var sale = new SaleForExcelReport(mySqlDataReader.GetString(0),
                income,
                mySqlDataReader.GetDecimal(3),
                currentTax);
            sales.Add(sale);
        }
    }
}