namespace SupermarketsChain.SalesByVendorReport
{
    using System;

    public class SqlServerToXMLSalesByVendorReport
    {
        static void Main()
        {
            //DateTime startDate = new DateTime(2015, 07, 27);
            //DateTime endDate = new DateTime(2015, 07, 28);
            var parser = new XmlReportsExporter();

            string input = Console.ReadLine();
            var arguments = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var datesRange = parser.ParseInputParams(arguments);

            parser.GenerateReport(datesRange);

        }
    }
}
