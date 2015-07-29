namespace SupermarketChain
{
    using System;

    public interface IXmlExport
    {
        string GenerateReport(DateTime startDate, DateTime endDate);

        string GenerateFileNameAndPath();
    }
}
