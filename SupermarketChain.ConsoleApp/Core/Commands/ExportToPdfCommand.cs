namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using System;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ExportToPdfCommand : AbstractCommand
    {
        public ExportToPdfCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var startDate = DateTime.Parse(commandArgs[1]);
            var endDate = DateTime.Parse(commandArgs[2]);
            var reporter = new SqlServerToPdfReport();
            this.Engine.OutputWriter.Write(reporter.ExportToPdf(startDate, endDate));
        }
    }
}
