namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using System;
    using Interfaces;

    public class ImportFromXmlCommand : AbstractCommand
    {
        public ImportFromXmlCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var parser = new XmlReportParser();
            var paths = new[]
                {
                    @"C:\Users\Rosen\Documents\GitHub\SupermarketsChain.git\trunk\Sources\sampleExpense1.xml",
                    @"C:\Users\Rosen\Documents\GitHub\SupermarketsChain.git\trunk\Sources\sampleExpense2.xml"
                };
            try
            {
                foreach (var path in paths)
                {
                    this.Engine.OutputWriter.Write(parser.ImportXmlExpenses(path));
                }
            }
            catch (Exception e)
            {
                this.Engine.OutputWriter.Write(e.Message);
            }
        }
    }
}
