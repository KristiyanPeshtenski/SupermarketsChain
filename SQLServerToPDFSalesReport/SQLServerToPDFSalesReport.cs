namespace SupermarketsChain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    using SQL = System.Data;

    public class SQLServerToPDFSalesReport
    {
        public static void Main()
        {
            var dbContext = new SupermarketsChainEntities();
            var startDate = Convert.ToDateTime("27/07/2015");
            var endDate = Convert.ToDateTime("29/07/2015");
            var salesReportData = dbContext.Sales
                .Where(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate)
                .GroupBy(
                    s => new
                             {
                                 ProductName = s.Product.Name,
                                 Location = s.Supermarket.Name,
                                 UnitPrice = s.Product.Price,
                                 Measure = s.Product.Measure.Name,
                                 Quantity = dbContext.Sales.Where(ss => ss.OrderedOn == s.OrderedOn).Count(ss => ss.ProductId == s.ProductId),
                                 Sum = s.Product.Price * dbContext.Sales.Where(ss => ss.OrderedOn == s.OrderedOn).Count(ss => ss.ProductId == s.ProductId),
                                 s.OrderedOn
                             })
                .Select(s => new
                                 {
                                     OrderedOn = s.Key.OrderedOn,
                                     Info = new
                                         {
                                            ProductName = s.Key.ProductName,
                                            Location = s.Key.Location,
                                            UnitPrice = s.Key.UnitPrice,
                                            Measure = s.Key.Measure,
                                            Quantity = s.Key.Quantity,
                                            Sum = s.Key.Sum
                                         }
                                     
                                 }).ToList();
            
            var salesTable = new SQL.DataTable("Sales");
            var totalSum = 0m;
            var grandTotalSum = 0m;
            HashSet<DateTime> dates = new HashSet<DateTime>();
            salesReportData.ForEach(d => dates.Add(d.OrderedOn));
            salesTable.Columns.Add("Date");
            salesTable.Columns.Add("Product");
            salesTable.Columns.Add("Quantity");
            salesTable.Columns.Add("Unit Price");
            salesTable.Columns.Add("Location");
            salesTable.Columns.Add("Sum");
            foreach (var date in dates)
            {
                salesTable.Rows.Add(date.ToShortDateString());
                for (int i = 0; i < salesReportData.Count(); i++)
                {
                    if (salesReportData.ElementAt(i).OrderedOn == date){   
                        salesTable.Rows.Add("",
                            salesReportData.ElementAt(i).Info.ProductName,
                            string.Format("{0:D}",salesReportData.ElementAt(i).Info.Quantity) + " " +
                            salesReportData.ElementAt(i).Info.Measure,
                            string.Format("{0:F}",salesReportData.ElementAt(i).Info.UnitPrice),
                            salesReportData.ElementAt(i).Info.Location,
                            string.Format("{0:F}",salesReportData.ElementAt(i).Info.Sum));

                        totalSum += salesReportData.ElementAt(i).Info.Sum;
                    }
                }

                salesTable.Rows.Add("", "", "", "", string.Format("Total sum for {0:d-MM-yyyy}:", date), string.Format("{0:F2}", totalSum));
                grandTotalSum += totalSum;
            }
            salesTable.Rows.Add("", "", "", "", "Total for the period:", string.Format("{0:F2}", grandTotalSum));
            ExportToPdf(salesTable);
        }

        public static void ExportToPdf(SQL.DataTable dt)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"C:\Users\Emily\Desktop\Sales-Report.pdf", FileMode.Create));
            document.Open();
            iTextSharp.text.Font font7 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7);
            iTextSharp.text.Font font7bold = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD);
            iTextSharp.text.Font font10 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);


            PdfPTable table = new PdfPTable(dt.Columns.Count);
            PdfPRow row = null;
            float[] widths = new float[] { 2f, 4f, 3f, 3f, 4f, 3f};

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            int iCol = 0;
            string colname = "";
            PdfPCell cell = new PdfPCell(new Phrase("Products"));

            cell.Colspan = dt.Columns.Count;

            PdfPCell header = new PdfPCell(new Phrase("Aggregated Sales Report", font10));
            header.Colspan = 6;
            header.HorizontalAlignment = 1;
            header.VerticalAlignment = 1;
            header.PaddingTop = 10;
            header.PaddingBottom = 10;
            table.AddCell(header);

            foreach (SQL.DataColumn c in dt.Columns)
            {
                //column headers
                table.AddCell(new PdfPCell(new Phrase(c.ColumnName, font7bold)) { BackgroundColor = new BaseColor(250, 200, 140), Padding = 2 });
            }

            foreach (SQL.DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    
                    table.AddCell(new PdfPCell(new Phrase(r[0].ToString(), font7)) { BackgroundColor = new BaseColor(250, 230, 200), Padding = 2 });
                    table.AddCell(new Phrase(r[1].ToString(), font7));
                    table.AddCell(new Phrase(r[2].ToString(), font7));
                    table.AddCell(new Phrase(r[3].ToString(), font7));
                    table.AddCell(new Phrase(r[4].ToString(), font7));
                    table.AddCell(new PdfPCell(new Phrase(r[5].ToString(), font7)) { BackgroundColor = new BaseColor(250, 230, 200), Padding = 2 });
                }
            } 
            document.Add(table);
            document.Close();
        }
    }
}
