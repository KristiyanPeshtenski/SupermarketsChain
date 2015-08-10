namespace SupermarketChain.ConsoleApp.Core
{
    using System;
    using System.Text;

    using SupermarketChain.ConsoleApp.Interfaces;

    public class ConsoleWriter : IOutputWriter
    {
        private readonly StringBuilder outputBuffer;

        public ConsoleWriter()
        {
            this.outputBuffer = new StringBuilder();
        }

        public bool AutoFlush { get; set; }

        public void Write(string line)
        {
            this.outputBuffer.AppendLine(line);

            if (this.AutoFlush)
            {
                this.Flush();
            }
        }

        public void Flush()
        {
            Console.Write(this.outputBuffer);
            this.outputBuffer.Clear();
        }
    }
}
