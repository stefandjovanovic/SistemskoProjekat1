using SistemskoProjekat1.Services;
using System.Net;

namespace SistemskoProjekat1
{
    public class Program
    {
        private static void Main(string[] args)
        {
            ConversionService conversionService = new ConversionService("C:\\Users\\najda\\OneDrive\\Documents\\Fax\\3. godina\\Sistemsko Prog\\konverzija");

            WebServer ws = new WebServer(conversionService.GetRequest, 20, "http://localhost:5050/");

            ws.Run();
            Console.WriteLine("A simple webserver. Press a key to quit.");
            Console.ReadKey();
            ws.Stop();
        }
    }
}