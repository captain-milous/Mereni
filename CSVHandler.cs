using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mereni
{
    /// <summary>
    /// Třída pro práci s CSV soubory. Obsahuje metody pro ukládání dat do CSV souborů.
    /// </summary>
    public static class CSVHandler
    {
        /// <summary>
        /// Objekt pro zamčení souboru při zápisu, aby nedošlo k současnému zápisu z více vláken.
        /// </summary>
        private static readonly object fileLock = new object();
        /// <summary>
        /// Uloží data do CSV souboru. Pokud soubor neexistuje, vytvoří jej a přidá záhlaví.
        /// Pokud soubor existuje, přidá data jako nový řádek.
        /// </summary>
        /// <param name="devid">Identifikátor zařízení.</param>
        /// <param name="data">Data k uložení, kde klíč je název sloupce a hodnota je hodnota pro daný sloupec.</param>
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

                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ";",
                    HasHeaderRecord = !fileExists
                };

                using (var writer = new StreamWriter(filePath, true))
                using (var csv = new CsvWriter(writer, config))
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
