namespace SupermarketChain.ConsoleApp
{
    using SupermarketChain.ConsoleApp.Core;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ProjectMain
    {
        public static void Main()
        {
            IInputReader consoleReader = new ConsoleReader();
            var consoleWriter = new ConsoleWriter
            {
                AutoFlush = true
            };

            ICommandManager commandManager = new CommandManager();

            var engine = new Engine(consoleReader, consoleWriter, commandManager);

            engine.Start();
        }
    }
}
