namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using SupermarketChain.ConsoleApp.Interfaces;

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
