using System;
using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core.Commands
{
    public class ImportFromOracleCommand : AbstractCommand
    {
        public ImportFromOracleCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            var importFromOracle = new OracleToSqlServer();
            this.Engine.OutputWriter.Write(importFromOracle.ReplicateDataFromOracle());
        }
    }
}
