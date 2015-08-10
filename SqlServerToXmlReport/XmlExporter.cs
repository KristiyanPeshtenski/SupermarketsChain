namespace SupermarketChain
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlExporter : IXmlExport
    {
        private const string ExportFileName = "SalesByVendor.xml";
        private const string DefaultDateFormat = "dd-MM-yyyy";
        private const string ExportXmlSuccess = "Export XML Sales by Vendor Reports: Done!";

        private readonly string defaultFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public XmlExporter()
        {
            this.Db = new SupermarketsChainSqlServerEntities();
            this.DefautFileLocation = this.defaultFileLocation;
            this.DefaultFileName = ExportFileName;
        }

        public SupermarketsChainSqlServerEntities Db { get; set; }

        public string DefaultFileName { get; private set; }

        public string DefautFileLocation { get; private set; }

        public string GenerateReport(DateTime startDate, DateTime endDate)
        {
            string path = this.GenerateFileNameAndPath();

            var salesByVendor = new XElement("Sales",
                this.Db.Sales.Where(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate)
                .GroupBy(gr => new { gr.Products.Vendors, gr.OrderedOn }).ToList()
                .Select(gr => new XElement("Sale",
                        new XAttribute("Vendor", gr.Key.Vendors.Name),
                        new XElement("Summary",
                            new XAttribute("Date",
                                gr.Key.OrderedOn.ToString(DefaultDateFormat, CultureInfo.InvariantCulture)),
                                  new XAttribute("TotalSales", gr.Sum(e => e.Products.Price).ToString("F"))))));

            salesByVendor.Save(path);
            return ExportXmlSuccess;
        }

        public string GenerateFileNameAndPath()
        {
            return this.DefautFileLocation +
                "\\" +
                DateTime.Now.ToString("dd-MM-yyyy") +
                " " +
                this.DefaultFileName;
        }
    }
}
