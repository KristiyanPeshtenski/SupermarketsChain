using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core.Commands
{
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
