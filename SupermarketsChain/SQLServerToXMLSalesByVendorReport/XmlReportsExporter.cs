using System.Xml.Linq;

namespace SupermarketsChain.SalesByVendorReport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Collections;
    using System.Globalization;

    public class XmlReportsExporter
    {
        private SupermarketsChainEntities db;
        private readonly string defaultFileLocation = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private const string defaultFileName = "SalesByVendor.xml";
        private const string defaultDateFormat = "dd-MM-yyyy";

        public XmlReportsExporter()
        {
            this.db = new SupermarketsChainEntities();
            this.DefautFileLocation = defaultFileLocation;
            this.DefaultFileName = defaultFileName;
        }

        public string DefaultFileName { get; private set; }

        public string DefautFileLocation { get; private set; }


        public IList<DateTime> ParseInputParams(string[] inputArgs)
        {
            try
            {
                var parsedArgs = new DateTime[2];
                var startDate = DateTime.ParseExact(inputArgs[0], defaultDateFormat, CultureInfo.InvariantCulture);
                var endDate = DateTime.ParseExact(inputArgs[1], defaultDateFormat, CultureInfo.InvariantCulture);
                if (startDate > endDate)
                {
                    throw new InvalidCastException("start date cannot be after endDate");
                }
                parsedArgs[0] = startDate;
                parsedArgs[1] = endDate;
                return parsedArgs;
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException ("Invalid input format.");
            }
        }

        public void GenerateReport(IList<DateTime> dateArgs)
        {
            DateTime startDate = dateArgs[0];
            DateTime endDate = dateArgs[1];
            string path = DefautFileLocation + "\\" + DefaultFileName;
            var salesByVendor = new XElement("Sales",
                this.db.Sales.Where(s => s.OrderedOn >= startDate && s.OrderedOn <= endDate)
                .GroupBy(gr => new { gr.Product.Vendor, gr.OrderedOn }).ToList()
                .Select(gr => new XElement("Sale",
                        new XAttribute("Vendor", gr.Key.Vendor.Name),
                        new XElement("Summary",
                            new XAttribute("Date",
                                gr.Key.OrderedOn.ToString(defaultDateFormat, CultureInfo.InvariantCulture)),
                            new XAttribute("TotalSales", gr.Sum(e => e.Product.Price).ToString("F")
                                )))));
           
            salesByVendor.Save(path);
        }
    }
}