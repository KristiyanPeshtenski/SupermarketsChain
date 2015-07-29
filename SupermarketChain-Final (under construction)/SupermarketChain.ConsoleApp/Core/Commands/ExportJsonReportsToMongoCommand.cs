namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using System;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ExportJsonReportsToMongoCommand : AbstractCommand
    {
        public ExportJsonReportsToMongoCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var startDate = DateTime.Parse(commandArgs[1]);
            var endDate = DateTime.Parse(commandArgs[2]);
            var exporter = new SalesByProductReport();
            this.Engine.OutputWriter.Write(exporter.ExportJson(startDate, endDate));
        }
    }
}
