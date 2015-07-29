namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ExitCommand : AbstractCommand
    {
        public ExitCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            this.Engine.Stop();
        }
    }
}
