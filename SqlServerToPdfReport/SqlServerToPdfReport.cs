namespace SupermarketChain
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    using iTextSharp.text;
    using iTextSharp.text.pdf;

    public class SqlServerToPdfReport
    {
        private const string ExportPdfReportSuccess = "Export PDF Sales Reports: Done!";

        private readonly string exportFileName = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Sales-Report.pdf";

        public string ExportToPdf(DateTime startDate, DateTime endDate)
        {
            DataTable dt = this.CreateTableForReport(startDate, endDate);
            Document document = new Document();
            var exportPath = this.exportFileName;
            PdfWriter.GetInstance(document, new FileStream(exportPath, FileMode.Create));
            document.Open();
            Font font7 = FontFactory.GetFont(FontFactory.HELVETICA, 7);
            Font font7bold = FontFactory.GetFont(FontFactory.HELVETICA, 7, Font.BOLD);
            Font font10 = FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD);


            PdfPTable table = new PdfPTable(dt.Columns.Count);
            float[] widths = new float[] { 2f, 4f, 3f, 3f, 4f, 3f };

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell(new Phrase("Products"));

            cell.Colspan = dt.Columns.Count;

            PdfPCell header = new PdfPCell(new Phrase("Aggregated Sales Report", font10));
            header.Colspan = 6;
            header.HorizontalAlignment = 1;
            header.VerticalAlignment = 1;
            header.PaddingTop = 10;
            header.PaddingBottom = 10;
            table.AddCell(header);

            foreach (DataColumn c in dt.Columns)
            {
                table.AddCell(new PdfPCell(new Phrase(c.ColumnName, font7bold)) { BackgroundColor = new BaseColor(250, 200, 140), Padding = 2 });
            }

            foreach (DataRow r in dt.Rows)
            {
                if (dt.Rows.Count > 0)
                {
                    table.AddCell(new PdfPCell(new Phrase(r[0].ToString(), font7bold))
                    {
                        Colspan = 6,
                        BackgroundColor = new BaseColor(250, 230, 200),
                        Padding = 2
                    });
                    table.AddCell(new Phrase(""));
                    table.AddCell(new Phrase(r[1].ToString(), font7));
                    table.AddCell(new Phrase(r[2].ToString(), font7));
                    table.AddCell(new Phrase(r[3].ToString(), font7));
                    table.AddCell(new Phrase(r[4].ToString(), font7));
                    table.AddCell(new PdfPCell(new Phrase(r[5].ToString(), font7))
                    {
                        BackgroundColor = new BaseColor(250, 200, 140),
                        Padding = 2
                    });
                }
            }
            document.Add(table);
            document.Close();
            return ExportPdfReportSuccess;
        }

        private IList<Sale> GetSalesData(DateTime startDate, DateTime endDate)
        {
            var dbContext = new SupermarketsChainSqlServerEntities();
            var salesReportData = dbContext.Sales
                .Where(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate)
                .GroupBy(
                    s => new
                    {
                        ProductName = s.Products.Name,
                        Location = s.Supermarkets.Name,
                        UnitPrice = s.Products.Price,
                        Measure = s.Products.Measures.Name,
                        Quantity = dbContext.Sales.Where(ss => ss.OrderedOn == s.OrderedOn).Count(ss => ss.ProductId == s.ProductId),
                        Sum = s.Products.Price * dbContext.Sales.Where(ss => ss.OrderedOn == s.OrderedOn).Count(ss => ss.ProductId == s.ProductId),
                        s.OrderedOn
                    })
                .Select(s => new Sale
                {
                    OrderedOn = s.Key.OrderedOn,
                    Info = new Info
                    {
                        ProductName = s.Key.ProductName,
                        Location = s.Key.Location,
                        UnitPrice = s.Key.UnitPrice,
                        Measure = s.Key.Measure,
                        Quantity = s.Key.Quantity,
                        Sum = s.Key.Sum
                    }
                })
                .ToList();

            return salesReportData;
        }

        private DataTable CreateTableForReport(DateTime startDate, DateTime endDate)
        {
            var salesReportData = this.GetSalesData(startDate, endDate);
            var salesTable = new DataTable("Sales");
            var totalSum = 0m;
            var grandTotalSum = 0m;
            var dates = new HashSet<DateTime>();
            salesReportData.ToList().ForEach(d => dates.Add(d.OrderedOn));
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
                    if (salesReportData.ElementAt(i).OrderedOn == date)
                    {
                        salesTable.Rows.Add("",
                            salesReportData.ElementAt(i).Info.ProductName,
                            string.Format("{0:D}", salesReportData.ElementAt(i).Info.Quantity) + " "
                            + salesReportData.ElementAt(i).Info.Measure,
                            string.Format("{0:F}", salesReportData.ElementAt(i).Info.UnitPrice),
                            salesReportData.ElementAt(i).Info.Location,
                            string.Format("{0:F}", salesReportData.ElementAt(i).Info.Sum));

                        totalSum += salesReportData.ElementAt(i).Info.Sum;
                    }
                }

                salesTable.Rows.Add("", "", "", "", string.Format("Total sum for {0:d-MM-yyyy}:", date), string.Format("{0:F2}", totalSum));
                grandTotalSum += totalSum;
            }
            salesTable.Rows.Add("", "", "", "", "Total for the period:", string.Format("{0:F2}", grandTotalSum));
            return salesTable;
        }
    }
}
