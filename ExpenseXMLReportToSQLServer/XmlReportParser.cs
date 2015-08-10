namespace SupermarketsChain.ExpenseXmlReportToSQLServer
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using System.IO;

    public class XmlReportParser : IXmlParser
    {
        private const string BasePath = "..\\..\\..\\..\\Sources\\";

        public XmlReportParser(SupermarketsChainEntities db)
        {
            this.Db = db;
        }

        public SupermarketsChainEntities Db { get; set; }


        public void ImportXmlExpenses(string file)
        {
            var path = BasePath + file;
            var vendors = this.Db.Vendors.Select(v => v);

            if (File.Exists(path))
            {
                var xmlReport = XDocument.Load(path);
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
                            this.Db.Expenses.Add(expence);
                            this.Db.SaveChanges();
                        }
                    }
                }

                Console.WriteLine("Report successfully imported");
            }
            else
            {
                throw new ArgumentException("File do not exist.");
            }
        }
    }
}