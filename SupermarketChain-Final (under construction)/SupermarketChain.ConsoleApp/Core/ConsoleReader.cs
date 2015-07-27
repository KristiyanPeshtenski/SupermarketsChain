using System;
using SupermarketChain.ConsoleApp.Interfaces;

namespace SupermarketChain.ConsoleApp.Core
{
    public class ConsoleReader : IInputReader
    {
        public string ReadNextLine()
        {
            return Console.ReadLine();
        }
    }
}
