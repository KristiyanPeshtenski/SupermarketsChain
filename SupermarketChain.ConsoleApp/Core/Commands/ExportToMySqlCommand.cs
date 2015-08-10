namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using SupermarketChain.ConsoleApp.Interfaces;

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
