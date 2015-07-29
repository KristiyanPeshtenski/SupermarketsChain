using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core.Commands
{
    public class ExportSqliteMysqlToExcel : AbstractCommand
    {
        public ExportSqliteMysqlToExcel(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var excelExporter = new SqliteMysqlToExcel();
            this.Engine.OutputWriter.Write(excelExporter.ExportDataSetToExcel());
        }
    }
}
