using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mereni
{
    public static class CSVHandler
    {
        private static readonly object fileLock = new object();

        public static void Save(string devid, Dictionary<string, string> data)
        {
            string directoryPath = Path.Combine(Program.Folder, devid);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string fileName = DateTime.Now.ToString("yyMMdd") + ".csv";
            string filePath = Path.Combine(directoryPath, fileName);

            lock (fileLock)
            {
                bool fileExists = File.Exists(filePath);

                using (var writer = new StreamWriter(filePath, true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    if (!fileExists)
                    {
                        csv.WriteField("Čas");
                        csv.WriteField("T0imp1");
                        csv.WriteField("T1imp1");
                        csv.WriteField("T0imp2");
                        csv.WriteField("T1imp2");
                        csv.WriteField("T0imp3");
                        csv.WriteField("T1imp3");
                        csv.WriteField("T0imp4");
                        csv.WriteField("T1imp4");
                        csv.NextRecord();
                    }

                    foreach (var item in data)
                    {
                        csv.WriteField(item.Value);
                    }
                    csv.NextRecord();
                }
            }
        }
    }
}
