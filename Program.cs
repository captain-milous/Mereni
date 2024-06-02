using System.Net;

namespace Mereni
{
    public class Program
    {
        public static string Folder = AppDomain.CurrentDomain.BaseDirectory + "Mereni";

        static async Task Main(string[] args)
        {
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

                while (true)
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
