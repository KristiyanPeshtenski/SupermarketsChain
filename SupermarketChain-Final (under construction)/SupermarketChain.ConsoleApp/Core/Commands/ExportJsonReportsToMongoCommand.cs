using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core.Commands
{
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
