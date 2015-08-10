namespace SupermarketChain.ConsoleApp.Interfaces
{
    public interface ICommand
    {
        IEngine Engine { get; }

        void Execute(string[] commandArgs);
    }
}
