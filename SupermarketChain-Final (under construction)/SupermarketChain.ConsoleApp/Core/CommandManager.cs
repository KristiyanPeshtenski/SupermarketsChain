namespace SupermarketChain.ConsoleApp.Core
{
    using System;
    using System.Collections.Generic;

    using SupermarketChain.ConsoleApp.Core.Commands;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class CommandManager : ICommandManager
    {
        protected readonly IDictionary<string, ICommand> CommandsByName;

        public CommandManager()
        {
            this.CommandsByName = new Dictionary<string, ICommand>();
        }

        public IEngine Engine { get; set; }

        public void ManageCommand(string[] commandArgs)
        {
            string commandName = commandArgs[0];
            if (!this.CommandsByName.ContainsKey(commandName))
            {
                throw new NotSupportedException(
                    "Command is not supported by engine");
            }

            var command = this.CommandsByName[commandName];
            command.Execute(commandArgs);
        }

        public void SeedCommands()
        {
            this.CommandsByName["export-to-excel"] = new ExportSqliteMysqlToExcel(this.Engine);
            this.CommandsByName["export-to-mysql"] = new ExportToMySqlCommand(this.Engine);
            this.CommandsByName["export-json"] = new ExportJsonReportsToMongoCommand(this.Engine);
            this.CommandsByName["export-to-xml"] = new ExportToXmlCommand(this.Engine);
            this.CommandsByName["export-to-pdf"] = new ExportToPdfCommand(this.Engine);
            this.CommandsByName["import-from-oracle"] = new ImportFromOracleCommand(this.Engine);
            this.CommandsByName["import-from-zip"] = new ImportFromZipCommand(this.Engine);
            this.CommandsByName["import-from-xml"] = new ImportFromXmlCommand(this.Engine);
            this.CommandsByName["exit"] = new ExitCommand(this.Engine);
        }
    }
}
