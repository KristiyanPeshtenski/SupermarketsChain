namespace SupermarketsChain
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using Ionic.Zip;

    public class ZipFileReader
    {
        public static void ReadZipFile(string filePath)
        {
            using (ZipFile zip = ZipFile.Read(filePath))
            {
                foreach (var file in zip)
                {
                    if (file.FileName.Contains(".xls"))
                    {
                        string pathInsideZip = file.FileName;
                        //Console.WriteLine("Title " + pathInsideZip);
                        Regex regex = new Regex(@"((?:[0-9]+){0,2}-[a-zA-Z]+-(?:[0-9]+){4})\.xls");
                        var dateOrder = regex.Match(pathInsideZip).Groups[1].Value;
                        //Console.WriteLine(dateOrder);
                        string fullPath = filePath.Replace("Sales-Reports.zip", "Sales") + '\\' + pathInsideZip.Replace('/', '\\');
                        file.Extract("C:../../../../Sources/Sales/", ExtractExistingFileAction.OverwriteSilently);
                        XlsFileReader.DateOrder = dateOrder;
                        XlsFileReader.ReadXls(fullPath);
                    }
                }
            }
        }

        public static void ReadDirectory(string filePath)
        {
            string[] subDirectories = Directory.GetDirectories(filePath);
            foreach (string subDir in subDirectories)
            {
                string[] xlsFiles = Directory.GetFiles(subDir);

                foreach (string xlsFile in xlsFiles)
                {
                    XlsFileReader.ReadXls(xlsFile);
                }
            }
        }
    }
}
