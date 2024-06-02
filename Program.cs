using System.Net;

namespace Mereni
{
    /// <summary>
    /// Hlavní třída programu, která obsahuje vstupní bod programu a definice základních proměnných.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Cesta ke složce, kde budou ukládána měření.
        /// </summary>
        public static string Folder = Path.Combine("/home/mites/Public", "Mereni");
        /// <summary>
        /// Příznak pro běh programu. Pokud je nastaven na false, program se ukončí.
        /// </summary>

        private static bool isRunning = true;
        /// <summary>
        /// Vstupní bod programu. Inicializuje HTTP listener a zpracovává příchozí HTTP požadavky.
        /// </summary>
        /// <param name="args">Argumenty příkazového řádku (nepoužívají se).</param>
        /// <returns>Asynchronní úloha.</returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine(Folder);
            try
            {
                if (!Directory.Exists(Folder))
                {
                    Directory.CreateDirectory(Folder);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://192.168.1.240:80/");

            try
            {
                listener.Start();
                Console.WriteLine("Listening...");

                while (isRunning)
                {
                    try
                    {
                        HttpListenerContext context = await listener.GetContextAsync();
                        HttpListenerRequest request = context.Request;
                        HttpListenerResponse response = context.Response;

                        if (request.HttpMethod == "GET")
                        {
                            var query = request.QueryString;
                            string devid = query["devid"];
                            string timestamp = DateTime.Now.ToString("HH:mm:ss");
                            var data = new Dictionary<string, string>
                            {
                                { "Timestamp", timestamp },
                                { "T0imp1", query["T0imp1"] },
                                { "T1imp1", query["T1imp1"] },
                                { "T0imp2", query["T0imp2"] },
                                { "T1imp2", query["T1imp2"] },
                                { "T0imp3", query["T0imp3"] },
                                { "T1imp3", query["T1imp3"] },
                                { "T0imp4", query["T0imp4"] },
                                { "T1imp4", query["T1imp4"] }
                            };
                            CSVHandler.Save(devid, data);
                            string responseString = "Data received and logged";
                            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                            response.ContentLength64 = buffer.Length;
                            Stream output = response.OutputStream;
                            output.Write(buffer, 0, buffer.Length);
                            output.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing request: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Critical error: {ex.Message}");
            }
            finally
            {
                if (listener.IsListening)
                {
                    listener.Stop();
                }
                listener.Close();
            }
        }
    }
}
