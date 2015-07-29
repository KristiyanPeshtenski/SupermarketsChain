using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core.Commands
{
    public class ExportToMySqlCommand : AbstractCommand
    {
        public ExportToMySqlCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var mysqlExporter = new SqlServerToMySql();
            this.Engine.OutputWriter.Write(mysqlExporter.ExportDataIntoMySql());
        }
    }
}
