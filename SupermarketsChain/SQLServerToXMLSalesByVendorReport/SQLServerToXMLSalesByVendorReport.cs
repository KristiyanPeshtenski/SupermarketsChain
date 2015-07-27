namespace SupermarketsChain.SalesByVendorReport
{
    using System;

    public class SqlServerToXMLSalesByVendorReport
    {
        static void Main()
        {
            IXmlExporter parser = new XmlExporter();
            
            string input = Console.ReadLine();
            var arguments = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var datesRange = parser.ParseInputParams(arguments);

            parser.GenerateReport(datesRange);

        }
    }
}
