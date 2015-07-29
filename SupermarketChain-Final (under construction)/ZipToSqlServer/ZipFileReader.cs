using System.IO;
using System.Text;

namespace SupermarketChain
{
    using System.Text.RegularExpressions;
    using Ionic.Zip;

    public class ZipFileReader
    {
        public string ReadZipFile(string filePath)
        {
            var output = new StringBuilder();
            output.Append("Importing from zip archive: ");
            using (var zip = ZipFile.Read(filePath))
            {
                foreach (var file in zip)
                {
                    if (file.FileName.Contains(".xls"))
                    {
                        string pathInsideZip = file.FileName;
                        Regex regex = new Regex(@"((?:[0-9]+){0,2}-[a-zA-Z]+-(?:[0-9]+){4})\.xls");
                        var dateOrder = regex.Match(pathInsideZip).Groups[1].Value;
                        string fullPath = filePath.Replace("Sales-Reports.zip", "Sales") + '\\' + pathInsideZip.Replace('/', '\\');
                        file.Extract(@"C:\Users\Rosen\Documents\GitHub\SupermarketsChain.git\trunk\Sources\Sales", ExtractExistingFileAction.OverwriteSilently);
                        var xlsFileReader = new XlsFileReader {DateOrder = dateOrder};
                        xlsFileReader.ReadXls(fullPath);
                        Directory.Delete(@"C:\Users\Rosen\Documents\GitHub\SupermarketsChain.git\trunk\Sources\Sales", true);
                    }
                }
            }
            output.Append("Done!");
            return output.ToString();
        }
    }
}
