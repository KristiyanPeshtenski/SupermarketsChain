namespace SupermarketChain
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlExporter : IXmlExport
    {
        private readonly string defaultFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private const string defaultFileName = "SalesByVendor.xml";
        private const string defaultDateFormat = "dd-MM-yyyy";

        public XmlExporter()
        {
            this.Db = new SupermarketsChainSqlServerEntities();
            this.DefautFileLocation = defaultFileLocation;
            this.DefaultFileName = defaultFileName;
        }

        public SupermarketsChainSqlServerEntities Db { get; set; }

        public string DefaultFileName { get; private set; }

        public string DefautFileLocation { get; private set; }

        public string GenerateReport(DateTime startDate, DateTime endDate)
        {
            string path = GenerateFileNameAndPath();

            var salesByVendor = new XElement("Sales",
                this.Db.Sales.Where(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate)
                .GroupBy(gr => new { gr.Products.Vendors, gr.OrderedOn }).ToList()
                .Select(gr => new XElement("Sale",
                        new XAttribute("Vendor", gr.Key.Vendors.Name),
                        new XElement("Summary",
                            new XAttribute("Date",
                                gr.Key.OrderedOn.ToString(defaultDateFormat, CultureInfo.InvariantCulture)),
                            new XAttribute("TotalSales", gr.Sum(e => e.Products.Price).ToString("F")
                                )))));

            salesByVendor.Save(path);
            return "Generating XML Sales by Vendor Reports: Done!";
        }

        public string GenerateFileNameAndPath()
        {
            return DefautFileLocation +
                "\\" +
                DateTime.Now.ToString("dd-MM-yyyy") +
                " " +
                DefaultFileName;
        }
    }
}
