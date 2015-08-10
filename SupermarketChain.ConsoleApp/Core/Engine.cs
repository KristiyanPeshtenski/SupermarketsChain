namespace SupermarketChain.ConsoleApp.Core
{
    using System;

    using Interfaces;

    public sealed class Engine : IEngine
    {
        private readonly ICommandManager commandManager;
        private readonly IInputReader reader;
        private readonly IOutputWriter writer;

        private bool isStarted;

        public Engine(IInputReader reader, IOutputWriter writer, ICommandManager commandManager)
        {
            this.writer = writer;
            this.reader = reader;
            this.commandManager = commandManager;
            this.commandManager.Engine = this;
        }

        public IOutputWriter OutputWriter
        {
            get { return this.writer; }
        }

        public void Start()
        {
            this.commandManager.SeedCommands();
            this.isStarted = true;

            while (this.isStarted)
            {
                string input = this.reader.ReadNextLine();
                string[] inputArgs = input.Split(' ');

                try
                {
                    this.commandManager.ManageCommand(inputArgs);
                }
                catch (Exception ex)
                {
                    this.writer.Write(ex.Message);
                }
            }

            this.writer.Flush();
        }

        public void Stop()
        {
            this.isStarted = false;
        }
    }
}
