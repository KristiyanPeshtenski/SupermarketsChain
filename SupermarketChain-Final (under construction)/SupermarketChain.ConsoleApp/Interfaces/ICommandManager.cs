namespace SupermarketChain.ConsoleApp.Interfaces
{
    public interface ICommandManager
    {
        IEngine Engine { get; set; }

        void ManageCommand(string[] commandArgs);

        void SeedCommands();
    }
}
