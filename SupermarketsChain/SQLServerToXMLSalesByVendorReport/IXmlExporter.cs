namespace SupermarketsChain.SalesByVendorReport
{
    using System;
    using System.Collections.Generic;

    public interface IXmlExporter
    {
        IList<DateTime> ParseInputParams(string[] inputArgs);

        void GenerateReport(IList<DateTime> args );
        
        string GenerateFileNameAndPath();
    }
}