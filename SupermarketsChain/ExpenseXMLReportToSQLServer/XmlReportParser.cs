namespace SupermarketsChain
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;

    public class XmlReportParser
    {
        public void ReadXmlFile(string path)
        {
            var context = new SupermarketsChainEntities();
            var vendors = context.Vendors.Select(e => e);
            // Load Xml file.
            var xmlReport = XDocument.Load(@path);
            var vendorNodes = xmlReport.XPathSelectElements("expenses-by-month/vendor");

            foreach (var vendorNode in vendorNodes)
            {
                var vendorName = vendorNode.Attribute("name").Value;
                var vendor = vendors.FirstOrDefault(v => v.Name == vendorName);
                var expensesNodes = vendorNode.Elements("expenses");

                if (vendor != null)
                {
                    foreach (var expensesNode in expensesNodes)
                    {
                        var month = DateTime.Parse(expensesNode.Attribute("month").Value);
                        decimal expense = decimal.Parse(expensesNode.Value);
                        var expence = new Expens
                        {
                            Month = month,
                            Expense = expense
                        };
                        expence.Vendors.Add(vendor);
                        context.Expenses.Add(expence);
                        context.SaveChanges();
                    }
                }
            }

            Console.WriteLine("Report successfully imported");
        }

        // <Summary>
        //      Validate data from xml file and check for null vlaues.
        // </Summary>
        // TODO: have to decide what happen when vendor expence by month are duplicate. 

        //public VendorExpensesByMonth ValidateXmlData(Vendor vendor, string month, decimal? expence)
        //{
        //    if (vendor == null)
        //    {
        //        throw new ArgumentException("Invalid vendor name.");
        //    }
        //    if (String.IsNullOrEmpty(month))
        //    {
        //        throw new ArgumentException("month cannot be empty");
        //    }

        //    if (expence == null)
        //    {
        //        throw new ArgumentException("Month expence cannot be null.");
        //    }

        //    return CreateExpenceByMonth(vendor, month, expence);
        //}
    }
}