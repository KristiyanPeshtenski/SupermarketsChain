namespace SupermarketChain.ConsoleApp.Core
{
    using System;
    using SupermarketChain.ConsoleApp.Interfaces;

    public class ConsoleReader : IInputReader
    {
        public string ReadNextLine()
        {
            return Console.ReadLine();
        }
    }
}
