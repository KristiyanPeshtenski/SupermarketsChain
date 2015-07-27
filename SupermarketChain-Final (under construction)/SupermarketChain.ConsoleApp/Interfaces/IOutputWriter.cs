namespace SupermarketChain.ConsoleApp.Interfaces
{
    public interface IOutputWriter
    {
        void Write(string line);

        void Flush();
    }
}
