using System;
using System.Collections.Generic;
using SupermarketChain.ConsoleApp.Core.Commands;
using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core
{
    public class CommandManager : ICommandManager
    {
        protected readonly IDictionary<string, ICommand> commandsByName;

        public CommandManager()
        {
            this.commandsByName = new Dictionary<string, ICommand>();
        }

        public IEngine Engine { get; set; }

        public void ManageCommand(string[] commandArgs)
        {
            string commandName = commandArgs[0];
            if (!this.commandsByName.ContainsKey(commandName))
            {
                throw new NotSupportedException(
                    "Command is not supported by engine");
            }

            var command = this.commandsByName[commandName];
            command.Execute(commandArgs);
        }

        public void SeedCommands()
        {
            this.commandsByName["import-from-oracle"] = new ImportFromOracleCommand(this.Engine);
            this.commandsByName["import-from-zip"] = new ImportFromZipCommand(this.Engine);
            this.commandsByName["import-from-xml"] = new ImportFromXmlCommand(this.Engine);
            this.commandsByName["exit"] = new ExitCommand(this.Engine);
        }
    }
}
