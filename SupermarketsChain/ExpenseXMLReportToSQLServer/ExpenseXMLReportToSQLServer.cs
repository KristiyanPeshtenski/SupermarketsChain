namespace SupermarketsChain
{
    using System;
    
    class ExpenseXMLReportToSQLServer
    {
        public static void Main()
        {
            var parser = new XmlReportParser();
            Console.WriteLine("Enter path to XML file.");
            string path = "path: ..\\..\\..\\..\\Sources\\sampleExpense1.xml";

            try
            {
                parser.ReadXmlFile(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
