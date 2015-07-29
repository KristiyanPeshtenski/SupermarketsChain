namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using SupermarketChain.ConsoleApp.Interfaces;

    public abstract class AbstractCommand : ICommand
    {
        protected AbstractCommand(IEngine engine)
        {
            this.Engine = engine;
        }

        public IEngine Engine { get; private set; }

        public abstract void Execute(string[] commandArgs);
    }
}
