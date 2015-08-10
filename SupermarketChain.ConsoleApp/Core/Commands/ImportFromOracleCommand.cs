namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using SupermarketChain.ConsoleApp.Interfaces;

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
