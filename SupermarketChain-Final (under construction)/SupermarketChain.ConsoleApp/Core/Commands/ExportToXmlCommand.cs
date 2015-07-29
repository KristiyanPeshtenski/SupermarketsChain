namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using System;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ExportToXmlCommand : AbstractCommand
    {
        public ExportToXmlCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var startDate = DateTime.Parse(commandArgs[1]);
            var endDate = DateTime.Parse(commandArgs[2]);
            var xmlExporter = new XmlExporter();
            this.Engine.OutputWriter.Write(xmlExporter.GenerateReport(startDate, endDate));
        }
    }
}
