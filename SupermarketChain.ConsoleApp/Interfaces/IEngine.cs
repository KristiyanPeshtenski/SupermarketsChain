namespace SupermarketChain.ConsoleApp.Interfaces
{
    public interface IEngine
    {
        void Start();

        void Stop();

        IOutputWriter OutputWriter { get; }
    }
}
