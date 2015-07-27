namespace SupermarketChain.ConsoleApp.Core.Commands
{
    using Interfaces;

    public class ImportFromZipCommand : AbstractCommand
    {
        public ImportFromZipCommand(IEngine engine) : base(engine)
        {
        }

        public override void Execute(string[] commandArgs)
        {
            const string filePath = @"C:\Users\Rosen\Documents\GitHub\SupermarketsChain.git\trunk\Sources\Sales-Reports.zip";
            var zipFileReader = new ZipFileReader();
            this.Engine.OutputWriter.Write(zipFileReader.ReadZipFile(filePath));
        }
    }
}
